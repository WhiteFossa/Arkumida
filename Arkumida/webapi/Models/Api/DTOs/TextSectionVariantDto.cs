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
    /// Variant content
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

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
        DateTime creationTime
    )
    {
        Id = id;
        Content = content; // It may be empty
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