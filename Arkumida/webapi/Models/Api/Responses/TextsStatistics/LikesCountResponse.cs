using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.TextsStatistics;

/// <summary>
/// Response with likes count
/// </summary>
public class LikesCountResponse
{
    /// <summary>
    /// Likes count
    /// </summary>
    [JsonPropertyName("likesCount")]
    public long LikesCount { get; private set; }

    public LikesCountResponse
    (
        long likesCount
    )
    {
        if (likesCount < 0)
        {
            throw new ArgumentException("Negative likes count!", nameof(likesCount));
        }

        LikesCount = likesCount;
    }
}