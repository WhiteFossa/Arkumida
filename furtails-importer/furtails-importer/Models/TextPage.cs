namespace furtails_importer.Models;

/// <summary>
/// Text page
/// </summary>
public class TextPage
{
    /// <summary>
    /// Page ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Page number, order pages by this value
    /// </summary>
    public int Number { get; set; }
    
    /// <summary>
    /// Text sections
    /// </summary>
    public IList<TextSection> Sections { get; set; }
}