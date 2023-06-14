using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums;

namespace webapi.Dao.Implementations;

public class TagsDao : ITagsDao
{
    private readonly MainDbContext _dbContext;

    public TagsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateTagAsync(TagDbo tag)
    {
        _ = tag ?? throw new ArgumentNullException(nameof(tag), "Tag must not be null.");
        
        await _dbContext
            .Tags
            .AddAsync(tag);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }

    public async Task<IReadOnlyCollection<TagDbo>> GetTagsAsync(TagSubtype? subtype = null)
    {
        IQueryable<TagDbo> tags = _dbContext
            .Tags;

        if (subtype.HasValue)
        {
            tags = tags
                .Where(t => t.Subtype == subtype.Value);
        }

        return await tags
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<TagDbo>> GetCategoryTagsAsync()
    {
        return await _dbContext
            .Tags
            .Where(t => t.IsCategory)
            .OrderBy(t => t.CategoryOrder)
            .ToListAsync();
    }

    public async Task<TagDbo> GetTagByIdAsync(Guid id)
    {
        return await _dbContext
            .Tags
            .SingleAsync(t => t.Id == id);
    }
}