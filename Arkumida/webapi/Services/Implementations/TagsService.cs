using webapi.Dao.Abstract;
using webapi.Dao.Models.Enums;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Api.Enums;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class TagsService : ITagsService
{
    private readonly ITagsDao _tagsDao;
    private readonly ITagsMapper _tagsMapper;

    private readonly IList<TagSizeCategory> _tagSizeCategories = new List<TagSizeCategory>()
    {
        TagSizeCategory.Cat0,
        TagSizeCategory.Cat1,
        TagSizeCategory.Cat2,
        TagSizeCategory.Cat3,
        TagSizeCategory.Cat4,
        TagSizeCategory.Cat5,
        TagSizeCategory.Cat6,
        TagSizeCategory.Cat7,
        TagSizeCategory.Cat8,
        TagSizeCategory.Cat9,
        TagSizeCategory.Cat10,
        TagSizeCategory.Cat11,
        TagSizeCategory.Cat12,
        TagSizeCategory.Cat13
    };

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
        var tags = _tagsMapper.Map(await _tagsDao.GetCategoryTagsAsync());

        await PostprocessTags(tags);

        return tags;
    }

    public async Task<IReadOnlyCollection<Tag>> GetAllTagsAsync(TagSubtype? subtype = null)
    {
        var tags = _tagsMapper.Map(await _tagsDao.GetTagsAsync(subtype));

        await PostprocessTags(tags);

        return tags;
    }

    public async Task<Tag> GetTagByIdAsync(Guid id)
    {
        var tag = _tagsMapper.Map(await _tagsDao.GetTagByIdAsync(id));

        await PostprocessTags(new List<Tag>() { tag });

        return tag;
    }

    public async Task<Tag> GetTagByNameAsync(string name)
    {
        var tag = _tagsMapper.Map(await _tagsDao.GetTagByNameAsync(name));

        await PostprocessTags(new List<Tag>() { tag });

        return tag;
    }

    public async Task CreateTagAsync(Tag tag)
    {
        _ = tag ?? throw new ArgumentNullException(nameof(tag), "Tag must be populated.");
        
        var dbTag = _tagsMapper.Map(tag);

        await _tagsDao.CreateTagAsync(dbTag);

        tag.Id = dbTag.Id; // DAO-generated ID
        
        // New tag is not used yet
        tag.TextsCount = 0;
        tag.SizeCategory = TagSizeCategory.Cat0;
    }

    public IReadOnlyCollection<Tag> OrderTags(IEnumerable<Tag> tags)
    {
        var result = new List<Tag>();
        
        result.AddRange(tags.Where(t => t.Subtype == TagSubtype.Participants).OrderBy(t => t.Name)); // 1) Participants
        result.AddRange(tags.Where(t => t.Subtype == TagSubtype.Species).OrderBy(t => t.Name)); // 2) Species
        result.AddRange(tags.Where(t => t.Subtype == TagSubtype.Setting).OrderBy(t => t.Name)); // 3) Setting
        result.AddRange(tags.Where(t => t.Subtype == TagSubtype.Actions).OrderBy(t => t.Name)); // 4) Actions

        if (result.Count != tags.Count())
        {
            throw new InvalidOperationException("There is a bug somewhere! Looks like new TagSubtype was not added here.");
        }

        return result;
    }

    private async Task CalculateTagPopularityAsync(Tag tag)
    {
        var random = new Random();
        tag.TextsCount = random.Next(0, 3000);
    }
    
    private async Task CalculateTagsPopularityAsync(IReadOnlyCollection<Tag> tags)
    {
        var popularityTasks = new List<Task>();
        foreach (var tag in tags)
        {
            popularityTasks.Add(CalculateTagPopularityAsync(tag));
        }

        Task.WaitAll(popularityTasks.ToArray());
    }

    private async Task PostprocessTags(IReadOnlyCollection<Tag> tags)
    {
        // Popularity have to be calculated among all tags, not only among given ones
        var dbTags = _tagsMapper.Map(await _tagsDao.GetTagsAsync());
        await CalculateTagsPopularityAsync(dbTags);
        
        // Tags, fed to method, may have no popularity, so take it from all tags collection
        foreach (var tag in tags)
        {
            tag.TextsCount = dbTags.Single(dt => dt.Id == tag.Id).TextsCount;
        }

        var mostPopularTagTextsCount = dbTags
            .Select(t => t.TextsCount)
            .Max();
        
        // Size categories
        foreach (var tag in tags)
        {
            var normalizedPopularity = tag.TextsCount / (float)mostPopularTagTextsCount; // Tag popularity, normalized to [0; 1] range

            var sizeCategoryIndex = (int)Math.Floor(normalizedPopularity * _tagSizeCategories.Count);
            if (sizeCategoryIndex > _tagSizeCategories.Count - 1)
            {
                sizeCategoryIndex = _tagSizeCategories.Count - 1;
            }
            
            tag.SizeCategory = _tagSizeCategories[sizeCategoryIndex];
        }

    }
    
}