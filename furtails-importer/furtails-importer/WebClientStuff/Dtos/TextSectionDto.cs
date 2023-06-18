using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class TextSectionDto
{
    /// <summary>
    /// Section ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Text in original language, usually in English. If this is Russian text, then original text is empty and the single variant
    /// contains actual text
    /// </summary>
    [JsonPropertyName("originalText")]
    public string OriginalText { get; set; }

    /// <summary>
    /// Sections are ordered by this field (to get a meaningful text)
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }
    
    /// <summary>
    /// Translation variants
    /// </summary>
    [JsonPropertyName("variants")]
    public IList<TextSectionVariantDto> Variants { get; set; }
}