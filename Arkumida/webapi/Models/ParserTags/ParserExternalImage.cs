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

using System.Text.RegularExpressions;
using webapi.Helpers;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserExternalImage : ParserTagBase
{
    private const string MatchRegexp = @"^\[img\](.+)\[/img\]";
    private readonly Regex _regexp = new Regex(MatchRegexp, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    public override string GetMatchString()
    {
        throw new NotImplementedException("Can't be implemented for regexp-matched tag!");
    }

    public override int GetRequestedTextLength()
    {
        return int.MaxValue;
    }

    public override Tuple<bool, int, IReadOnlyCollection<string>> TryMatch(string text)
    {
        var matches = _regexp.Matches(text);
        if (!matches.Any())
        {
            return new Tuple<bool, int, IReadOnlyCollection<string>>(false, 0, new string[] {});
        }

        var matchedContentLength = matches
            .First()
            .Length;

        var imageUrl = matches
            .First()
            .Groups
            .Values
            .ToList()[1] // Captured image url
            .Value;

        return new Tuple<bool, int, IReadOnlyCollection<string>>(true, matchedContentLength, new string[] { imageUrl });
    }

    public override void Action(List<TextElementDto> elements, string currentText, IReadOnlyCollection<string> matchGroups, IReadOnlyCollection<TextFile> textFiles)
    {
        var matchGroupsList = matchGroups.ToList();

        var imageUrl = matchGroupsList[0];
        
        // Image URL hash acts as unique image identifier
        var imageUrlHash = SHA512Helper.CalculateSHA512(imageUrl);
        
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText , new string[] {}));
        elements.Add(new TextElementDto(TextElementType.ExternalImage, "", new string[] { imageUrl, imageUrlHash }));
    }
}