using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserQuoteBegin : ExactMatchedParserTag
{
    private const string TextToMatch = "[q]";
    
    public override string GetMatchString()
    {
        return TextToMatch;
    }

    public override int GetRequestedTextLength()
    {
        return TextToMatch.Length;
    }

    public override void Action
    (
        List<TextElementDto> elements,
        string currentText,
        IReadOnlyCollection<string> matchGroups,
        IReadOnlyCollection<TextFile> textFiles
    )
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText, new string[] {}));
        elements.Add(new TextElementDto(TextElementType.QuoteBegin, "", new string[] {}));
        elements.Add(new TextElementDto(TextElementType.ParagraphBegin, "", new string[] {}));
    }
}