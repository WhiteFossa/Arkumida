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

    public TextSectionVariantDto
    (
        Guid id,
        string content,
        DateTime creationTime
    )
    {
        Id = id;
        
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content must not be empty.", nameof(content));
        }
        Content = content;

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