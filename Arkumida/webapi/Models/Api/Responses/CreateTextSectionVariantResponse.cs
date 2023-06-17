using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Text section variant creation response
/// </summary>
public class CreateTextSectionVariantResponse
{
    /// <summary>
    /// Variant
    /// </summary>
    [JsonPropertyName("variant")]
    public TextSectionVariantDto Variant { get; private set; }

    public CreateTextSectionVariantResponse
    (
        TextSectionVariantDto variant
    )
    {
        Variant = variant ?? throw new ArgumentNullException(nameof(variant), "Variant must be populated.");
    }
}