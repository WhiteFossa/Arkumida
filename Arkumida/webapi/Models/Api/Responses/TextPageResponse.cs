using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with information about one text page
/// </summary>
public class TextPageResponse
{
    /// <summary>
    /// Text data
    /// </summary>
    [JsonPropertyName("pageData")]
    public TextPageDto PageData { get; private set; }

    public TextPageResponse
    (
        TextPageDto pageData
    )
    {
        PageData = pageData ?? throw new ArgumentNullException(nameof(pageData), "Page data must not be null!");
    }
}