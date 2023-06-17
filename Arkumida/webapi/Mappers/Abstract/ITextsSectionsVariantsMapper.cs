using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for texts sections variants
/// </summary>
public interface ITextsSectionsVariantsMapper
{
    IReadOnlyCollection<TextSectionVariant> Map(IEnumerable<TextSectionVariantDbo> variants);

    TextSectionVariant Map(TextSectionVariantDbo variant);

    TextSectionVariantDbo Map(TextSectionVariant variant);

    IReadOnlyCollection<TextSectionVariantDbo> Map(IEnumerable<TextSectionVariant> variants);
}