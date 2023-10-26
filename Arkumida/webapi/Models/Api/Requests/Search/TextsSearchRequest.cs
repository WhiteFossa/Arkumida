using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Search;

/// <summary>
/// Request for texts search
/// </summary>
public class TextsSearchRequest
{
    /// <summary>
    /// Search query
    /// </summary>
    [JsonPropertyName("query")]
    public string Query { get; set; }

    /// <summary>
    /// Skip this amount of results
    /// </summary>
    [JsonPropertyName("skip")]
    public int Skip { get; set; }

    /// <summary>
    /// Take this amount of results
    /// </summary>
    [JsonPropertyName("take")]
    public int Take { get; set; }
}