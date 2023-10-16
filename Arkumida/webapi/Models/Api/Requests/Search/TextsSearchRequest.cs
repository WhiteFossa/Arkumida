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
}