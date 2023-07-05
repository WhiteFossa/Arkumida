using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// DTO for text section variant
/// </summary>
public class TextSectionVariantDto
{
    /// <summary>
    /// Variant ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Variant content (use it for variant creation, not for display)
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    /// <summary>
    /// Variant, parsed to elements. Use it for display.
    /// </summary>
    [JsonPropertyName("elements")]
    public IReadOnlyCollection<TextElementDto> Elements { get; set; }

    /// <summary>
    /// Variant creation time
    /// </summary>
    [JsonPropertyName("creationTime")]
    public DateTime CreationTime { get; set; }

    public TextSectionVariantDto()
    {
        
    }
    
    public TextSectionVariantDto
    (
        Guid id,
        string content,
        IReadOnlyCollection<TextElementDto> elements,
        DateTime creationTime
    )
    {
        Id = id;
        Content = content; // It may be empty
        Elements = elements ?? throw new ArgumentNullException(nameof(elements), "Elements collection must not be null.");
        CreationTime = creationTime;
    }

    /// <summary>
    /// To business-logic model
    /// </summary>
    public TextSectionVariant ToTextSectionVariant()
    {
        return new TextSectionVariant()
        {
            Id = Id,
            Content = Content,
            CreationTime = CreationTime
        };
    }
}