using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserLeftAlignedTextBegin : ParserTagBase
{
    public override string GetMatchString()
    {
        return "[l]";
    }

    public override void Action(List<TextElementDto> elements, string currentText)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText));
        elements.Add(new TextElementDto(TextElementType.LeftAlignedTextBegin, ""));
        elements.Add(new TextElementDto(TextElementType.ParagraphBegin, ""));
    }
}