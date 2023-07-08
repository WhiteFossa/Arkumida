namespace webapi.Models.ParserTags;

/// <summary>
/// Parser tag, matched exactly
/// </summary>
public abstract class ExactMatchedParserTag : ParserTagBase
{
    public override Tuple<bool, int, IReadOnlyCollection<string>> TryMatch(string text)
    {
        if (text == GetMatchString())
        {
            return new Tuple<bool, int, IReadOnlyCollection<string>>(true, GetRequestedTextLength(), new string[] {});
        }

        return new Tuple<bool, int, IReadOnlyCollection<string>>(false, 0, new string[] {});
    }
}