using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserStrikeOutTextBegin : ParserTagBase
{
    public override string GetMatchString()
    {
        return "[s]";
    }

    public override void Action(List<TextElementDto> elements, string currentText)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText));
        elements.Add(new TextElementDto(TextElementType.StrikeOutBegin, ""));
    }
}