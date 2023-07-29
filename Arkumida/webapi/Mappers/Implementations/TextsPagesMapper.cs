using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class TextsPagesMapper : ITextsPagesMapper
{
    private readonly ITextsSectionsMapper _sectionsMapper;

    public TextsPagesMapper(ITextsSectionsMapper sectionsMapper)
    {
        _sectionsMapper = sectionsMapper;
    }
    
    public IReadOnlyCollection<TextPage> Map(IEnumerable<TextPageDbo> pages)
    {
        if (pages == null)
        {
            return null;
        }

        return pages.Select(p => Map(p)).ToList();
    }

    public TextPage Map(TextPageDbo page)
    {
        if (page == null)
        {
            return null;
        }

        return new TextPage()
        {
            Id = page.Id,
            Number = page.Number,
            Sections = _sectionsMapper.Map(page.Sections).ToList()
        };
    }

    public TextPageDbo Map(TextPage page)
    {
        if (page == null)
        {
            return null;
        }

        return new TextPageDbo()
        {
            Id = page.Id,
            Number = page.Number,
            Sections = _sectionsMapper.Map(page.Sections).ToList()
        };
    }

    public IReadOnlyCollection<TextPageDbo> Map(IEnumerable<TextPage> pages)
    {
        if (pages == null)
        {
            return null;
        }

        return pages.Select(p => Map(p)).ToList();
    }
}