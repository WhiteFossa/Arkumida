using webapi.Models.Api.DTOs;

namespace webapi.Models;

/// <summary>
/// Text section variant, part of section
/// </summary>
public class TextSectionVariant
{
    /// <summary>
    /// Variant ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Variant content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Variant creation time
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// To DTO
    /// </summary>
    public TextSectionVariantDto ToDto()
    {
        return new TextSectionVariantDto(Id, Content, CreationTime);
    }
}