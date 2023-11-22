using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.TextsStatistics;

/// <summary>
/// Result of Unlike
/// </summary>
public class UnlikeResponse
{
    /// <summary>
    /// Is unliked successfully?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }

    public UnlikeResponse
    (
        bool isSuccessful
    )
    {
        IsSuccessful = isSuccessful;
    }
}