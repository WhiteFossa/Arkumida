using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// One text tag
/// </summary>
public class TextTagResponse
{
    /// <summary>
    /// Tag
    /// </summary>
    [JsonPropertyName("tag")]
    public TextTagDto Tag { get; private set; }

    public TextTagResponse
    (
        TextTagDto tag
    )
    {
        Tag = tag ?? throw new ArgumentNullException(nameof(tag), "Tag must be populated.");
    }
}