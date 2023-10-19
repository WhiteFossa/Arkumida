using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace webapi.OpenSearch.Helpers;

/// <summary>
/// This class able to "serialize"/"deserialize" GUIDs to/from hyphenless string. This is needed, because OpenSearch splits strings by hyphens when making Match queries.
/// Also it splits on digit-letter border too, so we must use plain numbers
/// </summary>
public static class OpenSearchGuidHelper
{
    public static string Serialize(Guid guid)
    {
        var asByteArray = guid.ToByteArray();

        var sb = new StringBuilder();

        foreach (var b in asByteArray)
        {
            sb.Append(b.ToString("D3"));
        }

        return sb.ToString();
    }

    public static Guid Deserialize(string guidAsString)
    {
        if (guidAsString.Length != 48)
        {
            throw new ArgumentException($"Incorrect string: {guidAsString}");
        }

        var bytes = Regex.Matches(guidAsString, @"\d{3}")
            .Select(m => m.Value)
            .Select(bs => byte.Parse(bs))
            .ToArray();

        return new Guid(bytes);
    }
}