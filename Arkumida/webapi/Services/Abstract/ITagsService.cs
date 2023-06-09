using webapi.Models;

namespace webapi.Services.Abstract;

public interface ITagsService
{
    /// <summary>
    /// Returns all tags, which are categories.
    /// </summary>
    Task<IReadOnlyCollection<Tag>> GetCategoriesTagsAsync();

    /// <summary>
    /// Get tag by ID
    /// </summary>
    Task<Tag> GetTagByIdAsync(Guid id);
}