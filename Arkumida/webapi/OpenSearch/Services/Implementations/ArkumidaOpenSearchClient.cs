using Microsoft.Extensions.Options;
using OpenSearch.Client;
using OpenSearch.Net;
using webapi.Models.Settings;
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
        connectionSettings.EnableDebugMode();
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
        var osId = await GetCreatureOpenSearchIdAsync(creature.DbId);
        
        var response = await _client.UpdateAsync<IndexableCreature>
        (
            osId,
            ic => ic
                .Doc(creature)
                .Index(IndexableCreature.IndexName)
        );

        if (!response.IsValid)
        {
            throw new InvalidOperationException($"Failed to update creature with DB ID = { creature.DbId }");
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
        
        var scrollResult = await _client
            .SearchAsync<IndexableText>
            (s => s
                .Index(IndexableText.IndexName)
                .Query
                (
                    q => q
                        .Bool
                        (b => b
                            .Must
                            (
                                // Title
                                q => q.MatchPhrase(m => m.Field(it => it.Title).Query(titleQuery ?? string.Empty)),
                                
                                // Description
                                q => q.MatchPhrase(m => m.Field(it => it.Description).Query(descriptionQuery ?? string.Empty)),
                                
                                // Content
                                q => q.MatchPhrase(m => m.Field(it => it.Content).Query(contentQuery ?? string.Empty)),
                                
                                // Author(s)
                                q => q.MatchPhrase
                                (m => m.Field(it => it.AuthorsDbIds)
                                    .Query(string.Join(" ", authors.Select(a => a.DbId.ToString())))
                                )
                            )
                        )
                )
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

    private async Task<IHit<IndexableCreature>> GetCreatureHitAsync(Guid creatureId)
    {
        var result = await _client.SearchAsync<IndexableCreature>
        (s => s
            .Index(IndexableCreature.IndexName)
            .Query(q => q
                .Match(m => m
                    .Field(ic => ic.DbId)
                    .Query(creatureId.ToString())))
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