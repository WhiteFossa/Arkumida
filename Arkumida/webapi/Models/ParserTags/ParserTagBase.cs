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
    /// TryMatch() requires this amount of text (but we may feed less if we are near the end of text)
    /// </summary>
    public abstract int GetRequestedTextLength();

    /// <summary>
    /// Tries to match given text, returns:
    /// 1) true if matched
    /// 2) Length of matched text
    /// 3) List of matched regexp groups
    /// </summary>
    public abstract Tuple<bool, int, IReadOnlyCollection<string>> TryMatch(string text);

    /// <summary>
    /// Action on current text, happened when tag matches
    /// </summary>
    public abstract void Action(List<TextElementDto> elements, string currentText, IReadOnlyCollection<string> matchGroups);
}