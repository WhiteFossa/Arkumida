#region License
// Arkumida - Furtails.pw next generation backend
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

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
    public abstract void Action
        (
            List<TextElementDto> elements,
            string currentText,
            IReadOnlyCollection<string> matchGroups,
            IReadOnlyCollection<TextFile> textFiles);

    /// <summary>
    /// If true, then we MUST NOT call TryMatch(), but have to skip to next tag. It is for performance
    /// </summary>
    public virtual bool IsFastSkip(string text)
    {
        return !text.Equals("[", StringComparison.InvariantCultureIgnoreCase);
    }
}