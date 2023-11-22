using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.TextsStatistics;

/// <summary>
/// Dislike action result
/// </summary>
public class DislikeResponse
{
    /// <summary>
    /// Is disliked successfully?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }
    
    public DislikeResponse
    (
        bool isSuccessful
    )
    {
        IsSuccessful = isSuccessful;
    }
}