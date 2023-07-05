using webapi.Models.Api.DTOs;

namespace webapi.Models.ParserTags;

/// <summary>
/// Base for tags parsing
/// </summary>
public abstract class ParserTagBase
{
    /// <summary>
    /// If we find in text this substring, then we have to process this tag
    /// </summary>
    public string Match { get; set; }

    /// <summary>
    /// Action on current text, happened when tag matches
    /// </summary>
    public abstract void Action(List<TextElementDto> elements, string currentText);
}