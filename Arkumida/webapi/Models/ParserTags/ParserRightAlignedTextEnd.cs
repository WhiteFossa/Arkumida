using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserRightAlignedTextEnd : ParserTagBase
{
    public override string GetMatchString()
    {
        return "[/r]";
    }

    public override void Action(List<TextElementDto> elements, string currentText)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText));
        elements.Add(new TextElementDto(TextElementType.ParagraphEnd, ""));
        elements.Add(new TextElementDto(TextElementType.RightAlignedTextEnd, ""));
    }
}