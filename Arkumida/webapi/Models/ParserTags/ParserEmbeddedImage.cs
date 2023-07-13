using System.Text.RegularExpressions;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models.ParserTags;

public class ParserEmbeddedImage : ParserTagBase
{
    private const string MatchRegexp = @"^\[bim\](.+)\[/bim\]";
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

        var imageName = matches
            .First()
            .Groups
            .Values
            .ToList()[1] // Captured image name
            .Value;

        return new Tuple<bool, int, IReadOnlyCollection<string>>(true, matchedContentLength, new string[] { imageName });
    }

    public override void Action(List<TextElementDto> elements, string currentText, IReadOnlyCollection<string> matchGroups, IReadOnlyCollection<TextFile> textFiles)
    {
        var image = textFiles
            .Single(tf => tf.Name == matchGroups.ToList()[0])
            .File;
        
        elements.Add(new TextElementDto(TextElementType.PlainText, currentText , new string[] {}));
        
        elements.Add(new TextElementDto(TextElementType.EmbeddedImage, "", new string[] { image.Id.ToString() }));
    }
}