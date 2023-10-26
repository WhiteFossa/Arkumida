using webapi.Models.Api.Responses.Search;

namespace webapi.Services.Abstract.Search;

/// <summary>
/// Service for texts search
/// </summary>
public interface ITextsSearchService
{
    /// <summary>
    /// Search for texts
    /// </summary>
    Task<TextsSearchResultsResponse> SearchTextsAsync(string query, int skip, int take);
}