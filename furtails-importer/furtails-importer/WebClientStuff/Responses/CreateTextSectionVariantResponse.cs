using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class CreateTextSectionVariantResponse
{
    /// <summary>
    /// Variant
    /// </summary>
    [JsonPropertyName("variant")]
    public TextSectionVariantDto Variant { get; set; }
}