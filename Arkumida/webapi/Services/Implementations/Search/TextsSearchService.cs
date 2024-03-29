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

using System.Text.RegularExpressions;
using webapi.Dao.Abstract;
using webapi.Models.Api.DTOs.Search;
using webapi.Models.Api.Responses.Search;
using webapi.OpenSearch.Services.Abstract;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Search;
using webapi.Services.Abstract.TextsStatistics;

namespace webapi.Services.Implementations.Search;

public class TextsSearchService : ITextsSearchService
{
    private readonly IArkumidaOpenSearchClient _arkumidaOpenSearchClient;
    private readonly ITextsDao _textsDao;
    private readonly ITextUtilsService _textUtilsService;
    private readonly ITextsStatisticsService _textsStatisticsService;

    public TextsSearchService
    (
        IArkumidaOpenSearchClient arkumidaOpenSearchClient,
        ITextsDao textsDao,
        ITextUtilsService textUtilsService,
        ITextsStatisticsService textsStatisticsService
    )
    {
        _arkumidaOpenSearchClient = arkumidaOpenSearchClient;
        _textsDao = textsDao;
        _textUtilsService = textUtilsService;
        _textsStatisticsService = textsStatisticsService;
    }
    
    public async Task<TextsSearchResultsResponse> SearchTextsAsync(string query, int skip, int take)
    {
        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), "Skip must not be negative.");
        }

        if (take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(take), "Take must be positive.");
        }
        
        // We can do nothing if creature entered an empty query
        if (string.IsNullOrWhiteSpace(query))
        {
            return new TextsSearchResultsResponse(query, new List<FoundTextDto>(), 0);
        }
        
        // Extracting query parts
        var titleQuery = ExtractTextTitleQuery(query);
        var descriptionQuery = ExtractTextDescriptionQuery(query);
        var contentQuery = ExtractTextContentQuery(query);
        var authorQuery = ExtractTextAuthorQuery(query);
        var tagsToIncludeQuery = ExtractTagsToInclude(query);
        var tagsToExcludeQuery = ExtractTagsToExclude(query);
        
        // Fallback
        if
        (
            titleQuery == null
            &&
            descriptionQuery == null
            &&
            contentQuery == null
            &&
            authorQuery == null
            &&
            !tagsToIncludeQuery.Any()
            &&
            !tagsToExcludeQuery.Any()
        )
        {
            titleQuery = query;
        }

        var openSearchResult = await _arkumidaOpenSearchClient.SearchForTextsAsync
            (
                titleQuery,
                descriptionQuery,
                contentQuery,
                authorQuery,
                tagsToIncludeQuery,
                tagsToExcludeQuery,
                skip,
                take
            );

        var textsIds = openSearchResult
            .Item1
            .Select(it => it.DbId)
            .ToList();

        var textsMetadata = await _textsDao.GetTextsMetadataByIdsAsync(textsIds);

        var texts = textsMetadata
            .Select(di => di.Value)
            .Select(async t => await _textUtilsService.PopulateTextMetadataAsync(t)) // TODO: Looking up metadata parts for each text is inoptimal. We can select all authors/translators/publishers and make one query
            .Select(t => t.Result);
        
        // Reordering texts as textGuids to keep the same order as in OpenSearch result
        texts = textsIds
            .Select(tg => texts.Single(t => t.Id == tg))
            .ToList();

        var readsCounts = await _textsStatisticsService.GetTextsReadsCountsAsync(textsIds);

        var foundTexts = texts
            .Select
            (t => new FoundTextDto
                (
                    t.Id,
                    t.CreateTime,
                    t.LastUpdateTime,
                    t.Title,
                    t.Description,
                    readsCounts[t.Id],
                    t.Tags.Select(tg => tg.ToTagDto()).ToList(),
                    t.IsIncomplete,
                    t.Authors.Select(a => a.ToDto()).ToList(),
                    t.Translators.Select(tr => tr.ToDto()).ToList(),
                    t.Publisher.ToDto()
                )
            )
            .ToList();
        
        return new TextsSearchResultsResponse(query, foundTexts, openSearchResult.Item2);
    }

    /// <summary>
    /// Extract text title query from full query. May return null if text title query is absent
    /// </summary>
    private string ExtractTextTitleQuery(string fullQuery)
    {
        var regexp = new Regex(@"(Название: \[(?<text_title>.+?)\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var matches = regexp.Matches(fullQuery);

        if (!matches.Any())
        {
            return null;
        }

        return matches
            .First()
            .Groups["text_title"]
            .Value
            .Trim();
    }
    
    /// <summary>
    /// Extract text description query from full query. May return null if text description query is absent
    /// </summary>
    private string ExtractTextDescriptionQuery(string fullQuery)
    {
        var regexp = new Regex(@"(Аннотация: \[(?<text_description>.+?)\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var matches = regexp.Matches(fullQuery);

        if (!matches.Any())
        {
            return null;
        }

        return matches
            .First()
            .Groups["text_description"]
            .Value
            .Trim();
    }
    
    /// <summary>
    /// Extract text content query from full query. May return null if text content query is absent
    /// </summary>
    private string ExtractTextContentQuery(string fullQuery)
    {
        var regexp = new Regex(@"(Текст: \[(?<text_content>.+?)\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var matches = regexp.Matches(fullQuery);

        if (!matches.Any())
        {
            return null;
        }

        return matches
            .First()
            .Groups["text_content"]
            .Value
            .Trim();
    }
    
    /// <summary>
    /// Extract text author query from full query. May return null if text author query is absent
    /// </summary>
    private string ExtractTextAuthorQuery(string fullQuery)
    {
        var regexp = new Regex(@"(Автор: \[(?<author>.+?)\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var matches = regexp.Matches(fullQuery);

        if (!matches.Any())
        {
            return null;
        }

        return matches
            .First()
            .Groups["author"]
            .Value
            .Trim();
    }

    /// <summary>
    /// Extract tags to include from full query. Will return empty collection if tags for include is not specified
    /// </summary>
    private IReadOnlyCollection<string> ExtractTagsToInclude(string fullQuery)
    {
        var regexp = new Regex(@"(\+Теги: \[(?<include_tags>.+?)\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var matches = regexp.Matches(fullQuery);

        if (!matches.Any())
        {
            return new List<string>();
        }
        
        return matches
            .First()
            .Groups["include_tags"]
            .Value
            .Split(",")
            .Select(tti => tti.Trim())
            .ToList();
    }
    
    /// <summary>
    /// Extract tags to exclude from full query. Will return empty collection if tags for exclude is not specified
    /// </summary>
    private IReadOnlyCollection<string> ExtractTagsToExclude(string fullQuery)
    {
        var regexp = new Regex(@"(\-Теги: \[(?<exclude_tags>.+?)\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var matches = regexp.Matches(fullQuery);

        if (!matches.Any())
        {
            return new List<string>();
        }
        
        return matches
            .First()
            .Groups["exclude_tags"]
            .Value
            .Split(",")
            .Select(tti => tti.Trim())
            .ToList();
    }
}