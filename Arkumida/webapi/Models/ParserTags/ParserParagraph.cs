using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

/// <summary>
/// Newline -> paragraph
/// </summary>
public class ParserParagraph : ExactMatchedParserTag
{
    public override string GetMatchString()
    {
        return Environment.NewLine;
    }

    public override int GetRequestedTextLength()
    {
        return Environment.NewLine.Length;
    }

    public override void Action(List<TextElementDto> elements, string currentText, IReadOnlyCollection<string> matchGroups)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText, new string[] {}));
        elements.Add(new TextElementDto(TextElementType.ParagraphEnd, "", new string[] {}));
        elements.Add(new TextElementDto(TextElementType.ParagraphBegin, "", new string[] {}));
    }
    
    public override bool IsFastSkip(string text)
    {
        return text != Environment.NewLine;
    }
}