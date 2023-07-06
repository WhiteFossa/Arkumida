using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserQuoteBegin : ParserTagBase
{
    public override string GetMatchString()
    {
        return "[q]";
    }

    public override void Action(List<TextElementDto> elements, string currentText)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText));
        elements.Add(new TextElementDto(TextElementType.QuoteBegin, ""));
        elements.Add(new TextElementDto(TextElementType.ParagraphBegin, ""));
    }
}