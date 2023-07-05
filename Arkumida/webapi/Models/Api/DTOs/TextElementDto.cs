using System.Text.Json.Serialization;
using webapi.Models.Enums;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Low level text element, all texts are lists of elements
/// </summary>
public class TextElementDto
{
    /// <summary>
    /// Element type
    /// </summary>
    [JsonPropertyName("type")]
    public TextElementType Type { get; set; }

    /// <summary>
    /// Element content, may be empty for contentless elements
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    public TextElementDto
    (
        TextElementType type,
        string content
    )
    {
        Type = type;
        Content = content;
    }
}