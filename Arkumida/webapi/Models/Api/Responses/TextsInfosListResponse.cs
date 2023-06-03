using System.Globalization;
using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// List of text infos
/// </summary>
public class TextsInfosListResponse
{
    /// <summary>
    /// Text infos
    /// </summary>
    [JsonPropertyName("textInfos")]
    public IReadOnlyCollection<TextInfoDto> TextInfos { get; private set; }

    public TextsInfosListResponse
    (
        IReadOnlyCollection<TextInfoDto> textInfos
    )
    {
        TextInfos = textInfos ?? throw new ArgumentNullException(nameof(textInfos), "Text infos must not be null");
    }
}