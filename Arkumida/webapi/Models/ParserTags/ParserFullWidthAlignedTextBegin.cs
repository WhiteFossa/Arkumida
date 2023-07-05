using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserFullWidthAlignedTextBegin : ParserTagBase
{
    public ParserFullWidthAlignedTextBegin()
    {
        Match = "[f]";
    }
    
    public override void Action(List<TextElementDto> elements, string currentText)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText));
        elements.Add(new TextElementDto(TextElementType.FullWidthAlignedTextBegin, ""));
        elements.Add(new TextElementDto(TextElementType.ParagraphBegin, ""));
    }
}