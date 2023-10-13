using System.Diagnostics;
using webapi.Dao.Abstract;
using webapi.Dao.Models.Enums;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Enums;
using webapi.OpenSearch.Models;
using webapi.OpenSearch.Services.Abstract;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class TagsService : ITagsService
{
    private readonly ITagsDao _tagsDao;
    private readonly ITagsMapper _tagsMapper;
    private readonly IArkumidaOpenSearchClient _arkumidaOpenSearchClient;

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
        ITagsMapper tagsMapper,
        IArkumidaOpenSearchClient arkumidaOpenSearchClient
    )
    {
        _tagsDao = tagsDao;
        _tagsMapper = tagsMapper;
        _arkumidaOpenSearchClient = arkumidaOpenSearchClient;
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
        
        // Indexing tag
        var tagToIndex = new IndexableTag()
        {
            DbId = tag.Id,
            Name = tag.Name
        };

        await _arkumidaOpenSearchClient.IndexTagAsync(tagToIndex);
    }

    public IReadOnlyCollection<Tag> OrderTags(IEnumerable<Tag> tags)
    {
        var result = new List<Tag>();
        
        result.AddRange(tags.Where(t => t.Subtype == TagSubtype.Participants).OrderBy(t => t.Name)); // 1) Participants
        result.AddRange(tags.Where(t => t.Subtype == TagSubtype.Species).OrderBy(t => t.Name)); // 2) Species
        result.AddRange(tags.Where(t => t.Subtype == TagSubtype.Setting).OrderBy(t => t.Name)); // 3) Setting
        result.AddRange(tags.Where(t => t.Subtype == TagSubtype.Actions).OrderBy(t => t.Name)); // 4) Actions
        
        // All other tags
        result.AddRange(tags
            .Where
            (
                t
                    =>
                t.Subtype != TagSubtype.Participants
                &&
                t.Subtype != TagSubtype.Species
                &&
                t.Subtype != TagSubtype.Setting
                &&
                t.Subtype != TagSubtype.Actions
            )
            .OrderBy(t => t.Name));

        if (result.Count != tags.Count())
        {
            throw new InvalidOperationException("There is a bug somewhere! Looks like new TagSubtype was not added here.");
        }

        return result;
    }
    
    private async Task PostprocessTags(IReadOnlyCollection<Tag> tags)
    {
        // Popularity
        var popularity = await _tagsDao.GetTagsPopularity(tags.Select(t => t.Id).ToList());
        foreach (var tag in tags)
        {
            tag.TextsCount = popularity[tag.Id];
        }
        
        // Size categories
        var mostPopularTagTextsCount = await _tagsDao.GetMaxTextsCountAsync();
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // We filter-out hidden tags, so it is possible that some tags (=some of hidden tags) will have more texts than most popular tag //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (mostPopularTagTextsCount == 0)
        {
            // DB is empty, import is not done yet
            foreach (var tag in tags)
            {
                tag.SizeCategory = TagSizeCategory.Cat0;
            }
        }
        else
        {
            var logMostPopularTagTextsCount = Math.Log10(mostPopularTagTextsCount + 1);
            foreach (var tag in tags)
            {
                var normalizedPopularity = Math.Log10(tag.TextsCount + 1) / logMostPopularTagTextsCount; // Tag popularity, normalized to [0; 1] range
                
                // Dirty protection against exceptions
                if (normalizedPopularity > 1)
                {
                    normalizedPopularity = 1;
                }
                else if (normalizedPopularity < 0)
                {
                    normalizedPopularity = 0;
                }
                
                var sizeCategoryIndex = (int)Math.Floor(normalizedPopularity * (_tagSizeCategories.Count - 1));
                tag.SizeCategory = _tagSizeCategories[sizeCategoryIndex];
            }
        }
    }
    
}