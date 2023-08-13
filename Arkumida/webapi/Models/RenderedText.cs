using webapi.Dao.Models.Enums.RenderedTexts;

namespace webapi.Models;

/// <summary>
/// Rendered text (business-logic model)
/// </summary>
public class RenderedText
{
    /// <summary>
    /// Rendered text ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// This text is rendered here
    /// </summary>
    public Text Text { get; set; }

    /// <summary>
    /// File type: plain text, PDF etc...
    /// </summary>
    public RenderedTextType Type { get; set; }
    
    /// <summary>
    /// Rendered text is stored here
    /// </summary>
    public File File { get; set; }
}