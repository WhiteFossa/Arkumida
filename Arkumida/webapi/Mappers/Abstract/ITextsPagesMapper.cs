using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for text pages
/// </summary>
public interface ITextsPagesMapper
{
    IReadOnlyCollection<TextPage> Map(IEnumerable<TextPageDbo> pages);

    TextPage Map(TextPageDbo page);

    TextPageDbo Map(TextPage page);

    IReadOnlyCollection<TextPageDbo> Map(IEnumerable<TextPage> pages);
}