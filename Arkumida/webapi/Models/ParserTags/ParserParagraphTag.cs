using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

/// <summary>
/// Newline -> paragraph
/// </summary>
public class ParserParagraphTag : ParserTagBase
{
    public ParserParagraphTag()
    {
        Match = "\n";
    }

    public override void Action(List<TextElementDto> elements, string currentText)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText));

        elements.Add(new TextElementDto(TextElementType.ParagraphEnd, ""));
        elements.Add(new TextElementDto(TextElementType.ParagraphBegin, ""));
    }
}