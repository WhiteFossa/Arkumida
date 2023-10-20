using Microsoft.Extensions.Options;
using OpenSearch.Client;
using OpenSearch.Net;
using webapi.Models.Settings;
using webapi.OpenSearch.Helpers;
using webapi.OpenSearch.Models;
using webapi.OpenSearch.Services.Abstract;
using ConnectionSettings = OpenSearch.Client.ConnectionSettings;

namespace webapi.OpenSearch.Services.Implementations;

public class ArkumidaOpenSearchClient : IArkumidaOpenSearchClient
{
    /// <summary>
    /// Keep scrolls open for given time
    /// </summary>
    private const string KeepScrollOpenTime = "60s";
    
    private readonly OpenSearchSettings _openSearchSettings;

    private OpenSearchClient _client;
    
    public ArkumidaOpenSearchClient
    (
        IOptions<OpenSearchSettings> openSearchSettings
    )
    {
        _openSearchSettings = openSearchSettings.Value;
        
        Connect();
    }
    
    public void Connect()
    {
        if (string.IsNullOrEmpty(_openSearchSettings.Url))
        {
            throw new InvalidOperationException("OpenSearch connection is not configured. Configure it in appsettings.json, please.");
        }

        if (_client != null)
        {
            return;
        }
        
        var connectionSettings = new ConnectionSettings(new Uri(_openSearchSettings.Url));
        //connectionSettings.EnableDebugMode(); // TODO: Do not forget to comment-me out
        _client = new OpenSearchClient(connectionSettings);
    }

    public async Task<string> IndexCreatureAsync(IndexableCreature creatureToIndex)
    {
        if (creatureToIndex == null)
        {
            throw new ArgumentNullException(nameof(creatureToIndex), "Attempt to index a null creature!");
        }
        
        var response = await _client
            .IndexAsync
            (
                creatureToIndex,
                i => i
                    .Index(IndexableCreature.IndexName)
            );

        if (!response.IsValid)
        {
            throw new InvalidOperationException($"Can't index a creature! Debug information: { response.DebugInformation }");
        }

        return response.Id;
    }
    
    public async Task<string> GetCreatureOpenSearchIdAsync(Guid creatureDbId)
    {
        return (await GetCreatureHitAsync(creatureDbId)).Id;
    }

    public async Task<IndexableCreature> GetCreatureByDbIdAsync(Guid creatureDbId)
    {
        return (await GetCreatureHitAsync(creatureDbId)).Source;
    }

    public async Task UpdateCreatureAsync(IndexableCreature creature)
    {
        var creatureDbId = OpenSearchGuidHelper.Deserialize(creature.DbId);
        var osId = await GetCreatureOpenSearchIdAsync(creatureDbId);
        
        var response = await _client.UpdateAsync<IndexableCreature>
        (
            osId,
            ic => ic
                .Doc(creature)
                .Index(IndexableCreature.IndexName)
        );

        if (!response.IsValid)
        {
            throw new InvalidOperationException($"Failed to update creature with DB ID = { creatureDbId }");
        }
    }

    public async Task<string> IndexTagAsync(IndexableTag tagToIndex)
    {
        if (tagToIndex == null)
        {
            throw new ArgumentNullException(nameof(tagToIndex), "Attempt to index a null tag!");
        }
        
        var response = await _client
            .IndexAsync
            (
                tagToIndex,
                i => i
                    .Index(IndexableTag.IndexName)
            );

        if (!response.IsValid)
        {
            throw new InvalidOperationException($"Can't index a tag! Debug information: { response.DebugInformation }");
        }

        return response.Id;
    }

    public async Task<string> IndexTextAsync(IndexableText textToIndex)
    {
        if (textToIndex == null)
        {
            throw new ArgumentNullException(nameof(textToIndex), "Attempt to index a null text!");
        }
        
        var response = await _client
            .IndexAsync
            (
                textToIndex,
                i => i
                    .Index(IndexableText.IndexName)
            );

        if (!response.IsValid)
        {
            throw new InvalidOperationException($"Can't index a text! Debug information: { response.DebugInformation }");
        }

        return response.Id;
    }

    public async Task<IReadOnlyCollection<IndexableText>> SearchForTextsAsync
    (
        string titleQuery,
        string descriptionQuery,
        string contentQuery,
        string authorQuery,
        IReadOnlyCollection<string> tagsToIncludeQuery,
        IReadOnlyCollection<string> tagsToExcludeQuery
    )
    {
        var result = new List<IndexableText>();
        
        var authors = await SearchForCreaturesAsync(authorQuery);

        var tagsToInclude = new List<IndexableTag>();
        foreach (var tagToIncludeQueryPart in tagsToIncludeQuery)
        {
            tagsToInclude.AddRange(await SearchForTagsAsync(tagToIncludeQueryPart));
        }

        var tagsToExclude = new List<IndexableTag>();
        foreach (var tagToExcludeQueryPart in tagsToExcludeQuery)
        {
            tagsToExclude.AddRange(await SearchForTagsAsync(tagToExcludeQueryPart));
        }
        
        var scrollResult = await _client
            .SearchAsync<IndexableText>
            (s => s
                .Index(IndexableText.IndexName)
                
                // Only these fields will be returned
                .Source
                (sf => sf
                    .Includes
                    (i => i 
                        .Fields
                        (
                            f => f.DbId
                        )
                    )
                )
                
                .Query
                (
                    q => q
                        .Bool
                        (b =>
                        {
                            b = b
                                .Must(qm =>
                                {
                                    var qcs = new List<QueryContainer>();
                                    
                                    // Title
                                    if (titleQuery != null)
                                    {
                                        qcs.Add(qm.MatchPhrase(m => m.Field(it => it.Title).Query(titleQuery)));
                                    }
                                    
                                    // Description
                                    if (descriptionQuery != null)
                                    {
                                        qcs.Add(qm.MatchPhrase(m => m.Field(it => it.Description).Query(descriptionQuery)));
                                    }
                                    
                                    // Content
                                    if (contentQuery != null)
                                    {
                                        qcs.Add(qm.MatchPhrase(m => m.Field(it => it.Content).Query(contentQuery)));
                                    }
                                    
                                    // Author(s)
                                    if (authors.Any())
                                    {
                                        qcs.Add(qm.Terms(t => t.Field(it => it.AuthorsDbIds).Terms(authors.Select(a => a.DbId).ToList())));
                                    }
                                    
                                    // Tags to include
                                    if (tagsToInclude.Any())
                                    {
                                        qcs.Add(qm.Terms(t => t.Field(it => it.TagsDbIds).Terms(tagsToInclude.Select(tti => tti.DbId).ToList())));
                                    }
                                    
                                    return new QueryContainer(new BoolQuery()
                                    {
                                        Must = qcs
                                    });
                                });

                            // Tags to exclude
                            if (tagsToExclude.Any())
                            {
                                b = b
                                    .MustNot(qmn => qmn.Terms(t => t.Field(it => it.TagsDbIds).Terms(tagsToExclude.Select(tti => tti.DbId).ToList())));
                            }
                            
                            return b;
                        })
                )
                
                // To have reproduceable skip/take and have most recent stories on to
                .Sort(s => s.Descending(it => it.LastUpdateTime))
                
                .Scroll(KeepScrollOpenTime)
            );

        if (!scrollResult.IsValid)
        {
            throw new InvalidOperationException($"Text search failed! Debug information: { scrollResult.DebugInformation }");
        }

        while (scrollResult.Documents.Any()) 
        {
            result.AddRange(scrollResult.Documents);
            
            scrollResult = _client.Scroll<IndexableText>(KeepScrollOpenTime, scrollResult.ScrollId);
        }

        return result;
    }

    public async Task<IReadOnlyCollection<IndexableCreature>> SearchForCreaturesAsync(string displayNameQuery)
    {
        var result = new List<IndexableCreature>();
        
        var scrollResult = await _client
            .SearchAsync<IndexableCreature>
            (s => s
                .Index(IndexableCreature.IndexName)
                .Query
                (q => q
                    .MatchPhrase
                    (m => m
                        .Field(ic => ic.DisplayName)
                        .Query(displayNameQuery ?? string.Empty)
                    )
                )
                .Scroll(KeepScrollOpenTime)
            );

        if (!scrollResult.IsValid)
        {
            throw new InvalidOperationException($"Creature search failed! Debug information: { scrollResult.DebugInformation }");
        }

        while (scrollResult.Documents.Any()) 
        {
            result.AddRange(scrollResult.Documents);
            
            scrollResult = _client.Scroll<IndexableCreature>(KeepScrollOpenTime, scrollResult.ScrollId);
        }

        return result;
    }

    public async Task<IReadOnlyCollection<IndexableTag>> SearchForTagsAsync(string tagNameQuery)
    {
        var result = new List<IndexableTag>();
        
        var scrollResult = await _client
            .SearchAsync<IndexableTag>
            (s => s
                .Index(IndexableTag.IndexName)
                .Query
                (q => q
                    .MatchPhrase(m => m.Field(it => it.Name).Query(tagNameQuery ?? string.Empty))
                )
                .Scroll(KeepScrollOpenTime)
            );

        if (!scrollResult.IsValid)
        {
            throw new InvalidOperationException($"Tag search failed! Debug information: { scrollResult.DebugInformation }");
        }

        while (scrollResult.Documents.Any()) 
        {
            result.AddRange(scrollResult.Documents);
            
            scrollResult = _client.Scroll<IndexableTag>(KeepScrollOpenTime, scrollResult.ScrollId);
        }

        return result;
    }

    private async Task<IHit<IndexableCreature>> GetCreatureHitAsync(Guid creatureId)
    {
        var result = await _client.SearchAsync<IndexableCreature>
        (s => s
            .Index(IndexableCreature.IndexName)
            .Query(q => q
                .Match(m => m
                    .Field(ic => ic.DbId)
                    .Query(OpenSearchGuidHelper.Serialize(creatureId))))
        );

        if (!result.IsValid)
        {
            throw new InvalidOperationException($"Unable to get creature hit, creature DbId={ creatureId }");
        }

        return result
            .Hits
            .Single();
    }
}