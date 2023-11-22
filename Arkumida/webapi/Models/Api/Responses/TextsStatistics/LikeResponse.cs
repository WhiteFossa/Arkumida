using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.TextsStatistics;

/// <summary>
/// Result of Like
/// </summary>
public class LikeResponse
{
    /// <summary>
    /// Is liked successfully?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }

    public LikeResponse
    (
        bool isSuccessful
    )
    {
        IsSuccessful = isSuccessful;
    }
}