using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with information, required to display read text page
/// </summary>
public class TextReadResponse
{
    /// <summary>
    /// Text data
    /// </summary>
    [JsonPropertyName("textData")]
    public TextReadDto TextData { get; private set; }
    
    public TextReadResponse
    (
        TextReadDto textData
    )
    {
        TextData = textData ?? throw new ArgumentNullException(nameof(textData), "TextData must not be null.");
    }
}