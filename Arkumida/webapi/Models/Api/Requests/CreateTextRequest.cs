using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to create a new text
/// </summary>
public class CreateTextRequest
{
    /// <summary>
    /// Text
    /// </summary>
    [JsonPropertyName("text")]
    public TextDto Text { get; set; }
}