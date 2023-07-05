using webapi.Models.Api.DTOs;

namespace webapi.Models.ParserTags;

/// <summary>
/// Base for tags parsing
/// </summary>
public abstract class ParserTagBase
{
    /// <summary>
    /// Returns tag content
    /// </summary>
    /// <returns></returns>
    public abstract string GetMatchString();

    /// <summary>
    /// Action on current text, happened when tag matches
    /// </summary>
    public abstract void Action(List<TextElementDto> elements, string currentText);
}