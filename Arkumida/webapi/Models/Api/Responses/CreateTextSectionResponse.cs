using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Text section creation response
/// </summary>
public class CreateTextSectionResponse
{
    /// <summary>
    /// Section
    /// </summary>
    [JsonPropertyName("section")]
    public TextSectionDto Section { get; private set; }

    public CreateTextSectionResponse
    (
        TextSectionDto section
    )
    {
        Section = section ?? throw new ArgumentNullException(nameof(section), "Section must be populated.");
    }
}