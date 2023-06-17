using webapi.Dao.Abstract;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class TextsSectionsVariantsService : ITextsSectionsVariantsService
{
    private readonly ITextsSectionsVariantsDao _variantsDao;
    private readonly ITextsSectionsVariantsMapper _variantsMapper;

    public TextsSectionsVariantsService
    (
        ITextsSectionsVariantsDao variantsDao,
        ITextsSectionsVariantsMapper variantsMapper
    )
    {
        _variantsDao = variantsDao;
        _variantsMapper = variantsMapper;
    }
    
    public async Task CreateVariantAsync(TextSectionVariant variant)
    {
        _ = variant ?? throw new ArgumentNullException(nameof(variant), "Variant must be populated.");
        
        var dbVariant = _variantsMapper.Map(variant);
        dbVariant.Id = Guid.Empty;

        await _variantsDao.CreateTextSectionVariantAsync(dbVariant);

        variant.Id = dbVariant.Id; // DAO-generated ID
    }
}