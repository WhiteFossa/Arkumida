using System.Text.RegularExpressions;
using webapi.Models.Api.DTOs.Search;
using webapi.Models.Api.Responses.Search;
using webapi.Services.Abstract.Search;

namespace webapi.Services.Implementations.Search;

public class TextsSearchService : ITextsSearchService
{
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
        
        
        // TODO: Replace me with good answer
        return new TextsSearchResultsResponse(query, new List<FoundTextDto>());
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
}