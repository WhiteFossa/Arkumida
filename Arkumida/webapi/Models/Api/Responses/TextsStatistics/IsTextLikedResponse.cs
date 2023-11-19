using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.TextsStatistics;

/// <summary>
/// Is text liked response
/// </summary>
public class IsTextLikedResponse
{
    /// <summary>
    /// Is text liked?
    /// </summary>
    [JsonPropertyName("isLiked")]
    public bool IsLiked { get; private set; }

    public IsTextLikedResponse
    (
        bool isLiked
    )
    {
        IsLiked = isLiked;
    }
}