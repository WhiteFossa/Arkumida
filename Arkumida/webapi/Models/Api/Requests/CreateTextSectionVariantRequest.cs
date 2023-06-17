using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to create a new text section variant
/// </summary>
public class CreateTextSectionVariantRequest
{
    /// <summary>
    /// Variant
    /// </summary>
    [JsonPropertyName("variant")]
    public TextSectionVariantDto Variant { get; set; }
}