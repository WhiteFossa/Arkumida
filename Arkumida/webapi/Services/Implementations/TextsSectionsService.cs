using webapi.Dao.Abstract;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class TextsSectionsService : ITextsSectionsService
{
    private readonly ITextsSectionsDao _sectionsDao;
    private readonly ITextsSectionsMapper _sectionsMapper;

    public TextsSectionsService
    (
        ITextsSectionsDao sectionsDao,
        ITextsSectionsMapper sectionsMapper
    )
    {
        _sectionsDao = sectionsDao;
        _sectionsMapper = sectionsMapper;
    }
    
    public async Task CreateSectionAsync(TextSection section)
    {
        _ = section ?? throw new ArgumentNullException(nameof(section), "Section mustn't be null.");

        var dbSection = _sectionsMapper.Map(section);
        dbSection.Id = Guid.Empty;

        await _sectionsDao.CreateTextSectionAsync(dbSection);

        section.Id = dbSection.Id;
    }

    public async Task AddVariantToSection(Guid sectionId, Guid variantId)
    {
        await _sectionsDao.AddVariantToSection(sectionId, variantId);
    }
}