using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for texts sections
/// </summary>
public interface ITextsSectionsMapper
{
    IReadOnlyCollection<TextSection> Map(IEnumerable<TextSectionDbo> sections);

    TextSection Map(TextSectionDbo section);

    TextSectionDbo Map(TextSection section);

    IReadOnlyCollection<TextSectionDbo> Map(IEnumerable<TextSection> sections);
}