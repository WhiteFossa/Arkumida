using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Services.Abstract;

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
    public TextSectionVariantDto ToDto(IReadOnlyCollection<TextFile> textFiles, ITextUtilsService textUtilsService)
    {
        return new TextSectionVariantDto
        (
            Id,
            string.Empty, // We don't need to pass content to frontend
            textUtilsService.ParseTextToElements(Content, textFiles),
            CreationTime
        );
    }
}