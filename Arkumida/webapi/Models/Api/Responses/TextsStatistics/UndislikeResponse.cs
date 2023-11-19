using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.TextsStatistics;

/// <summary>
/// Undislike action result
/// </summary>
public class UndislikeResponse
{
    /// <summary>
    /// Is undisliked successfully?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }
    
    public UndislikeResponse
    (
        bool isSuccessful
    )
    {
        IsSuccessful = isSuccessful;
    }
}