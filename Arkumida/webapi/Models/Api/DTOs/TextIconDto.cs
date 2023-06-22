using System.Text.Json.Serialization;
using webapi.Models.Enums;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Text icon
/// </summary>
public class TextIconDto
{
    /// <summary>
    /// Icon type
    /// </summary>
    [JsonPropertyName("type")]
    public TextIconType Type { get; private set; }

    /// <summary>
    /// Optional URL (if set, then icon will be a link)
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; private set; }

    public TextIconDto
    (
        TextIconType type,
        string url = ""
    )
    {
        Type = type;
        Url = url;
    }
}