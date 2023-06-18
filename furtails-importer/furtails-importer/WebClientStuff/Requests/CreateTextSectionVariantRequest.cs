using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

public class CreateTextSectionVariantRequest
{
    /// <summary>
    /// Variant
    /// </summary>
    [JsonPropertyName("variant")]
    public TextSectionVariantDto Variant { get; set; }
}