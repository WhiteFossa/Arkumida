using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Response with creature's about information (splitted to elements)
/// </summary>
public class AboutInfoAsElementsResponse
{
    /// <summary>
    /// Elements
    /// </summary>
    [JsonPropertyName("elements")]
    public IReadOnlyCollection<TextElementDto> Elements { get; set; }

    public AboutInfoAsElementsResponse
    (
        IReadOnlyCollection<TextElementDto> elements
    )
    {
        Elements = elements ?? throw new ArgumentNullException(nameof(elements), "About info elements must not be null");
    }
}