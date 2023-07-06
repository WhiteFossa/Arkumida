using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserTitleBegin : ParserTagBase
{
    public override string GetMatchString()
    {
        return "[h]";
    }

    public override void Action(List<TextElementDto> elements, string currentText)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText));
        elements.Add(new TextElementDto(TextElementType.TitleBegin, ""));
    }
}