#region License
// Arkumida - Furtails.pw next generation backend
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using Microsoft.Extensions.Options;
using OpenSearch.Client;
using webapi.Models.Settings;
using webapi.OpenSearch.Models;
using webapi.OpenSearch.Services.Abstract;

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

        // We use Keyword search, so we have to make Name lowercase to have case-insensitive search
        var processedIndexableTag = new IndexableTag()
        {
            DbId = tagToIndex.DbId,
            Name = tagToIndex.Name.ToLower()
        };
        
        var response = await _client
            .IndexAsync
            (
                processedIndexableTag,
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

    public async Task<Tuple<IReadOnlyCollection<IndexableText>, long>> SearchForTextsAsync
    (
        string titleQuery,
        string descriptionQuery,
        string contentQuery,
        string authorQuery,
        IReadOnlyCollection<string> tagsToIncludeQuery,
        IReadOnlyCollection<string> tagsToExcludeQuery,
        int skip,
        int take
    )
    {
        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), "Skip must not be negative.");
        }

        if (take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(take), "Take must be positive.");
        }

        List<IndexableCreature> authors = new List<IndexableCreature>();
        if (authorQuery != null)
        {
            authors = (await SearchForCreaturesAsync(authorQuery)).ToList();

            if (!authors.Any())
            {
                // No authors found, so we wouldn't have any result
                return new Tuple<IReadOnlyCollection<IndexableText>, long>(new List<IndexableText>(), 0);
            }
        }

        var tagsToInclude = new List<IndexableTag>();
        if (tagsToIncludeQuery.Any())
        {
            foreach (var tagToIncludeQueryPart in tagsToIncludeQuery)
            {
                var foundTags = await SearchForTagsAsync(tagToIncludeQueryPart);
                if (!foundTags.Any())
                {
                    // Non-existent tag required, we wouldn't have any results
                    return new Tuple<IReadOnlyCollection<IndexableText>, long>(new List<IndexableText>(), 0);
                }
                
                tagsToInclude.AddRange(foundTags);
            }
        }
        
        var tagsToExclude = new List<IndexableTag>();
        foreach (var tagToExcludeQueryPart in tagsToExcludeQuery)
        {
            tagsToExclude.AddRange(await SearchForTagsAsync(tagToExcludeQueryPart));
        }
        
        var searchResult = await _client
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
                                    if (authorQuery != null)
                                    {
                                        qcs.Add(qm.Terms(t => t.Field(it => it.AuthorsDbIds.Suffix("keyword")).Terms(authors.Select(a => a.DbId).ToList())));
                                    }
                                    
                                    // Tags to include
                                    if (tagsToIncludeQuery.Any())
                                    {
                                        foreach (var tagToInclude in tagsToInclude)
                                        {
                                            qcs.Add(qm.MatchPhrase(m => m.Field(it => it.TagsDbIds.Suffix("keyword")).Query(tagToInclude.DbId.ToString())));
                                        }
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
                                    .MustNot(qmn => qmn.Terms(t => t.Field(it => it.TagsDbIds.Suffix("keyword")).Terms(tagsToExclude.Select(tti => tti.DbId).ToList())));
                            }
                            
                            return b;
                        })
                )
                
                // To have reproduceable skip/take and have most recent stories on to
                .Sort(s => s.Descending(it => it.LastUpdateTime))
                
                .From(skip)
                .Size(take)
            );

        if (!searchResult.IsValid)
        {
            throw new InvalidOperationException($"Text search failed! Debug information: { searchResult.DebugInformation }");
        }

        return new Tuple<IReadOnlyCollection<IndexableText>, long>(searchResult.Documents, searchResult.Total);
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
        
        await _client.ClearScrollAsync(new ClearScrollRequest(scrollResult.ScrollId));
        
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
                    .MatchPhrase(m => m.Field(it => it.Name.Suffix("keyword")).Query(tagNameQuery.ToLower() ?? string.Empty))
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
        
        await _client.ClearScrollAsync(new ClearScrollRequest(scrollResult.ScrollId));

        return result;
    }

    private async Task<IHit<IndexableCreature>> GetCreatureHitAsync(Guid creatureId)
    {
        var result = await _client.SearchAsync<IndexableCreature>
        (s => s
            .Index(IndexableCreature.IndexName)
            .Query(q => q
                .Match(m => m
                    .Field(ic => ic.DbId.Suffix("keyword"))
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