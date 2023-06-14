using webapi.Dao.Models.Enums;
using webapi.Models;

namespace webapi.Services.Abstract;

public interface ITagsService
{
    /// <summary>
    /// Returns all tags, which are categories.
    /// </summary>
    Task<IReadOnlyCollection<Tag>> GetCategoriesTagsAsync();

    /// <summary>
    /// Get all existing tags, applying subtype filter if needed
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Tag>> GetAllTagsAsync(TagSubtype? subtype = null);
    
    /// <summary>
    /// Get tag by ID
    /// </summary>
    Task<Tag> GetTagByIdAsync(Guid id);

    /// <summary>
    /// Create new tag
    /// </summary>
    Task CreateTagAsync(Tag tag);
}