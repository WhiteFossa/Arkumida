using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Requests;

public class AddVariantToSectionRequest
{
    /// <summary>
    /// Add variant to this section
    /// </summary>
    [JsonPropertyName("sectionId")]
    public Guid SectionId { get; set; }

    /// <summary>
    /// Add this variant to section
    /// </summary>
    [JsonPropertyName("variantId")]
    public Guid VariantId { get; set; }
}