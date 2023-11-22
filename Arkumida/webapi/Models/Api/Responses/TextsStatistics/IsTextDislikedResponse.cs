using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.TextsStatistics;

/// <summary>
/// Is text disliked response
/// </summary>
public class IsTextDislikedResponse
{
    /// <summary>
    /// Is text disliked?
    /// </summary>
    [JsonPropertyName("isDisliked")]
    public bool IsDisliked { get; private set; }

    public IsTextDislikedResponse
    (
        bool isDisliked
    )
    {
        IsDisliked = isDisliked;
    }
}