namespace furtails_importer.Helpers;

public static class TextsHelper
{
    public static string FixupText(string text)
    {
        if (text == null)
        {
            return string.Empty;
        }
        
        return text
            .Replace('\u0000', ' ');
    }
}