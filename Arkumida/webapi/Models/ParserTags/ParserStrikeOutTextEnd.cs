using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserStrikeOutTextEnd : ExactMatchedParserTag
{
    private const string TextToMatch = "[/s]";
    
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
        elements.Add(new TextElementDto(TextElementType.StrikeOutEnd, "", new string[] {}));
    }
}