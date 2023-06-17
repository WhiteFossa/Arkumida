using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class TextsSectionsVariantsMapper : ITextsSectionsVariantsMapper
{
    public IReadOnlyCollection<TextSectionVariant> Map(IEnumerable<TextSectionVariantDbo> variants)
    {
        if (variants == null)
        {
            return null;
        }

        return variants.Select(v => Map(v)).ToList();
    }

    public TextSectionVariant Map(TextSectionVariantDbo variant)
    {
        if (variant == null)
        {
            return null;
        }

        return new TextSectionVariant()
        {
            Id = variant.Id,
            Content = variant.Content,
            CreationTime = variant.CreationTime
        };
    }

    public TextSectionVariantDbo Map(TextSectionVariant variant)
    {
        if (variant == null)
        {
            return null;
        }

        return new TextSectionVariantDbo()
        {
            Id = variant.Id,
            Content = variant.Content,
            CreationTime = variant.CreationTime
        };
    }

    public IReadOnlyCollection<TextSectionVariantDbo> Map(IEnumerable<TextSectionVariant> variants)
    {
        if (variants == null)
        {
            return null;
        }

        return variants.Select(v => Map(v)).ToList();
    }
}