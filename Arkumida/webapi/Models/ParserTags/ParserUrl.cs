using System.Text.RegularExpressions;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserUrl : ParserTagBase
{
    private const string FastSkipMatch = @"[url]";
    
    private const string MatchRegexp = @"^\[url\](\S+)\[/url\]";
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

        var urlContent = matches
            .First()
            .Groups
            .Values
            .ToList()[1] // Captured URL
            .Value;

        return new Tuple<bool, int, IReadOnlyCollection<string>>(true, matchedContentLength, new string[] { urlContent });
    }

    public override void Action(List<TextElementDto> elements, string currentText, IReadOnlyCollection<string> matchGroups)
    {
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText , new string[] {}));
        
        elements.Add(new TextElementDto(TextElementType.UrlBegin, "", new string[] { matchGroups.First() }));
        elements.Add(new TextElementDto(TextElementType.PlainText, matchGroups.First(), new string[] {}));
        elements.Add(new TextElementDto(TextElementType.UrlEnd, "", new string[] { }));
    }
}