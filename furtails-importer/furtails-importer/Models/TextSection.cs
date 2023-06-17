namespace furtails_importer.Models;

public class TextSection
{
    /// <summary>
    /// Text in original language
    /// </summary>
    public string OriginalText { get; set; }

    /// <summary>
    /// Variants of translation
    /// </summary>
    public List<TextSectionVariant> Variants { get; set; }
}