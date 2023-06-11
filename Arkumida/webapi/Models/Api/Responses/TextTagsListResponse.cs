using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with tags
/// </summary>
public class TextTagsListResponse
{
    /// <summary>
    /// Tags
    /// </summary>
    [JsonPropertyName("tags")]
    public IReadOnlyCollection<TextTagDto> Tags { get; private set; }

    public TextTagsListResponse
    (
        IReadOnlyCollection<TextTagDto> tags
    )
    {
        Tags = tags ?? throw new ArgumentNullException(nameof(tags), "Tags must be populated");
    }
}