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

    /// <summary>
    /// How many texts remaining (total count - (skip + take))
    /// </summary>
    [JsonPropertyName("remainingTexts")]
    public int RemainingTexts { get; private set; }

    public TextsInfosListResponse
    (
        IReadOnlyCollection<TextInfoDto> textInfos,
        int remainingTexts
    )
    {
        TextInfos = textInfos ?? throw new ArgumentNullException(nameof(textInfos), "Text infos must not be null");

        if (remainingTexts < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(remainingTexts), "Remaining texts count must be non-negative");
        }

        RemainingTexts = remainingTexts;
    }
}