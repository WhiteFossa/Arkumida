using System.Text.RegularExpressions;
using webapi.Dao.Abstract;
using webapi.Models.Api.DTOs.Search;
using webapi.Models.Api.Responses.Search;
using webapi.OpenSearch.Helpers;
using webapi.OpenSearch.Services.Abstract;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Search;

namespace webapi.Services.Implementations.Search;

public class TextsSearchService : ITextsSearchService
{
    private readonly IArkumidaOpenSearchClient _arkumidaOpenSearchClient;
    private readonly ITextsDao _textsDao;
    private readonly ITextUtilsService _textUtilsService;

    public TextsSearchService
    (
        IArkumidaOpenSearchClient arkumidaOpenSearchClient,
        ITextsDao textsDao,
        ITextUtilsService textUtilsService
    )
    {
        _arkumidaOpenSearchClient = arkumidaOpenSearchClient;
        _textsDao = textsDao;
        _textUtilsService = textUtilsService;
    }
    
    public async Task<TextsSearchResultsResponse> SearchTextsAsync(string query)
    {
        // We can do nothing if creature entered an empty query
        if (string.IsNullOrWhiteSpace(query))
        {
            return new TextsSearchResultsResponse(query, new List<FoundTextDto>());
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
                tagsToExcludeQuery
            );

        var textsGuids = openSearchResult
            .Select(it => OpenSearchGuidHelper.Deserialize(it.DbId))
            .ToList();

        var textsMetadata = await _textsDao.GetTextsMetadataByIdsAsync(textsGuids);

        var texts = textsMetadata
            .Select(di => di.Value)
            .Select(async t => await _textUtilsService.PopulateTextMetadataAsync(t))
            .Select(t => t.Result);

        var foundTexts = texts
            .Select
            (t => new FoundTextDto
                (
                    t.Id,
                    t.CreateTime,
                    t.LastUpdateTime,
                    t.Title,
                    t.Description,
                    t.ReadsCount,
                    t.VotesCount,
                    t.VotesPlus,
                    t.VotesMinus,
                    t.Tags.Select(tg => tg.ToTagDto()).ToList(),
                    t.IsIncomplete,
                    t.Authors.Select(a => a.ToDto()).ToList(),
                    t.Translators.Select(tr => tr.ToDto()).ToList(),
                    t.Publisher.ToDto()
                )
            )
            .ToList();
        
        return new TextsSearchResultsResponse(query, foundTexts);
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