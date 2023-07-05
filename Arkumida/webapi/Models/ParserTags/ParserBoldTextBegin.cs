using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserBoldTextBegin : ParserTagBase
{
    public override string GetMatchString()
    {
        return "[b]";
    }

    public override void Action(List<TextElementDto> elements, string currentText)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText));
        elements.Add(new TextElementDto(TextElementType.BoldBegin, ""));
    }
}