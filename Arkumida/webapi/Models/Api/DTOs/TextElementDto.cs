using System.Collections.ObjectModel;
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

    /// <summary>
    /// Element parameters (as a set of strings)
    /// </summary>
    [JsonPropertyName("parameters")]
    public IReadOnlyCollection<string> Parameters { get; set; }

    public TextElementDto
    (
        TextElementType type,
        string content,
        IReadOnlyCollection<string> parameters)
    {
        Type = type;
        Content = content;
        Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters), "Parameters must not be empty!");
    }
}