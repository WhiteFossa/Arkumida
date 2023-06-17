using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Text section DTO
/// </summary>
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

    public TextSectionDto()
    {
        
    }
    
    public TextSectionDto
    (
        Guid id,
        string originalText,
        int order,
        IReadOnlyCollection<TextSectionVariantDto> variants)
    {
        Id = id;
        OriginalText = originalText; // It may be empty
        Order = order;
        Variants = (variants ?? throw new ArgumentNullException(nameof(variants), "Variants must be populated.")).ToList();
    }

    public TextSection ToTextSection()
    {
        return new TextSection()
        {
            Id = Id,
            OriginalText = OriginalText,
            Order = Order,
            Variants = Variants.Select(v => v.ToTextSectionVariant()).ToList()
        };
    }
}