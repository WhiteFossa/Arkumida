using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for texts
/// </summary>
public interface ITextsMapper
{
    IReadOnlyCollection<Text> Map(IEnumerable<TextDbo> texts);

    Text Map(TextDbo text);

    TextDbo Map(Text text);

    IReadOnlyCollection<TextDbo> Map(IEnumerable<Text> texts);
}