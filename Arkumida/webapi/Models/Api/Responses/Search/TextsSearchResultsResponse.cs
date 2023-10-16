using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs.Search;

namespace webapi.Models.Api.Responses.Search;

/// <summary>
/// Response with texts search results
/// </summary>
public class TextsSearchResultsResponse
{
    /// <summary>
    /// This request was made
    /// </summary>
    [JsonPropertyName("query")]
    public string Query { get; private set; }

    /// <summary>
    /// Found texts
    /// </summary>
    [JsonPropertyName("foundTexts")]
    public IReadOnlyCollection<FoundTextDto> FoundTexts { get; private set; }

    public TextsSearchResultsResponse
    (
        string query,
        IReadOnlyCollection<FoundTextDto> foundTexts)
    {
        // Query string may be empty, however we will not return any text in this case
        Query = query ?? throw new ArgumentNullException(nameof(query), "Query must not be null!");

        FoundTexts = foundTexts ?? throw new ArgumentNullException(nameof(foundTexts), "Found texts must not be null!");
    }
}