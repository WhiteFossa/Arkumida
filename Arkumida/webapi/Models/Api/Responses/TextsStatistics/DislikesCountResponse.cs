using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.TextsStatistics;

/// <summary>
/// Response with dislikes count
/// </summary>
public class DislikesCountResponse
{
    /// <summary>
    /// Dislikes count
    /// </summary>
    [JsonPropertyName("dislikesCount")]
    public long DislikesCount { get; private set; }
    
    public DislikesCountResponse
    (
        long dislikesCount
    )
    {
        if (dislikesCount < 0)
        {
            throw new ArgumentException("Negative dislikes count!", nameof(dislikesCount));
        }

        DislikesCount = dislikesCount;
    }   
}