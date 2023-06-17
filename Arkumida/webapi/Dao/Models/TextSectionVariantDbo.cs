using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

/// <summary>
/// Text section consist of this variants, the last one is used as text source
/// </summary>
public class TextSectionVariantDbo
{
    /// <summary>
    /// Variant ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Variant content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Variant creation time
    /// </summary>
    public DateTime CreationTime { get; set; }
}