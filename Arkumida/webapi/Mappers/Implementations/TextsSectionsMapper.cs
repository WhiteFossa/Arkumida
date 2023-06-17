using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class TextsSectionsMapper : ITextsSectionsMapper
{
    private readonly ITextsSectionsVariantsMapper _variantsMapper;

    public TextsSectionsMapper(ITextsSectionsVariantsMapper variantsMapper)
    {
        _variantsMapper = variantsMapper;
    }
    
    public IReadOnlyCollection<TextSection> Map(IEnumerable<TextSectionDbo> sections)
    {
        if (sections == null)
        {
            return null;
        }

        return sections.Select(s => Map(s)).ToList();
    }

    public TextSection Map(TextSectionDbo section)
    {
        if (section == null)
        {
            return null;
        }

        return new TextSection()
        {
            Id = section.Id,
            OriginalText = section.OriginalText,
            Order = section.Order,
            Variants = _variantsMapper.Map(section.Variants).ToList()
        };
    }

    public TextSectionDbo Map(TextSection section)
    {
        if (section == null)
        {
            return null;
        }

        return new TextSectionDbo()
        {
            Id = section.Id,
            OriginalText = section.OriginalText,
            Order = section.Order,
            Variants = _variantsMapper.Map(section.Variants).ToList()
        };
    }

    public IReadOnlyCollection<TextSectionDbo> Map(IEnumerable<TextSection> sections)
    {
        if (sections == null)
        {
            return null;
        }

        return sections.Select(s => Map(s)).ToList();
    }
}