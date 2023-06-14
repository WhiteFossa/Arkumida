using webapi.Dao.Abstract;
using webapi.Dao.Models.Enums;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class TagsService : ITagsService
{
    private readonly ITagsDao _tagsDao;
    private readonly ITagsMapper _tagsMapper;

    public TagsService
    (
        ITagsDao tagsDao,
        ITagsMapper tagsMapper
    )
    {
        _tagsDao = tagsDao;
        _tagsMapper = tagsMapper;
    }

    public async Task<IReadOnlyCollection<Tag>> GetCategoriesTagsAsync()
    {
        return _tagsMapper.Map(await _tagsDao.GetCategoryTagsAsync());
    }

    public async Task<IReadOnlyCollection<Tag>> GetAllTagsAsync(TagSubtype? subtype = null)
    {
        return _tagsMapper.Map(await _tagsDao.GetTagsAsync(subtype));
    }

    public async Task<Tag> GetTagByIdAsync(Guid id)
    {
        return _tagsMapper.Map(await _tagsDao.GetTagByIdAsync(id));
    }

    public async Task CreateTagAsync(Tag tag)
    {
        var dbTag = _tagsMapper.Map(tag);

        await _tagsDao.CreateTagAsync(dbTag);

        tag.Id = dbTag.Id; // DAO-generated ID
    }
}