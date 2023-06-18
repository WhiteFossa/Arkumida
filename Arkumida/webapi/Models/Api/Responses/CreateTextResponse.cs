using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Text creation response
/// </summary>
public class CreateTextResponse
{
    /// <summary>
    /// Text
    /// </summary>
    [JsonPropertyName("text")]
    public TextDto Text { get; private set; }
    
    public CreateTextResponse
    (
        TextDto text
    )
    {
        Text = text ?? throw new ArgumentNullException(nameof(text), "Text must be populated.");
    }
}