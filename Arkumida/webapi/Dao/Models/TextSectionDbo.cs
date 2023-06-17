using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

/// <summary>
/// Text section
/// </summary>
public class TextSectionDbo
{
    /// <summary>
    /// Section ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Text in original language, usually in English. If this is Russian text, then original text is empty and the single variant
    /// contains actual text
    /// </summary>
    public string OriginalText { get; set; }

    /// <summary>
    /// Translation variants
    /// </summary>
    public IList<TextSectionVariantDbo> Variants { get; set; }
}