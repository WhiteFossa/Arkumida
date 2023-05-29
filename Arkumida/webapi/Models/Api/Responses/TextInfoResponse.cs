using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Text info
/// </summary>
public class TextInfoResponse
{
    /// <summary>
    /// Text info
    /// </summary>
    [JsonPropertyName("textInfo")]
    public TextInfoDto TextInfo { get; private set; }

    public TextInfoResponse
    (
        TextInfoDto textInfo
    )
    {
        TextInfo = textInfo ?? throw new ArgumentNullException(nameof(textInfo), "TextInfo must not be null");
    }
}