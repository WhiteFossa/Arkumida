using System.ComponentModel.DataAnnotations;
using webapi.Dao.Models.Enums.RenderedTexts;

namespace webapi.Dao.Models;

/// <summary>
/// Rendered text (e.g. txt or pdf file)
/// </summary>
public class RenderedTextDbo
{
    /// <summary>
    /// Rendered text ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// This text is rendered here
    /// </summary>
    public TextDbo Text { get; set; }

    /// <summary>
    /// File type: plain text, PDF etc...
    /// </summary>
    public RenderedTextType Type { get; set; }
    
    /// <summary>
    /// Rendered text is stored here
    /// </summary>
    public FileDbo File { get; set; }
}