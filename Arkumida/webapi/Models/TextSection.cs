using webapi.Models.Api.DTOs;
using webapi.Services.Abstract;

namespace webapi.Models;

/// <summary>
/// Text section
/// </summary>
public class TextSection
{
    /// <summary>
    /// Section ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Text in original language, usually in English. If this is Russian text, then original text is empty and the single variant
    /// contains actual text
    /// </summary>
    public string OriginalText { get; set; }
    
    /// <summary>
    /// Sections are ordered by this field (to get a meaningful text)
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Translation variants
    /// </summary>
    public IList<TextSectionVariant> Variants { get; set; }

    public TextSectionDto ToDto(ITextsService textsService)
    {
        return new TextSectionDto(Id, OriginalText, Order, Variants.Select(v => v.ToDto(textsService)).ToList());
    }
}