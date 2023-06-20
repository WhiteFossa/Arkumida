using webapi.Dao.Models;
using webapi.Dao.Models.Enums;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with tags
/// </summary>
public interface ITagsDao
{
    #region Create / update

    /// <summary>
    /// Create new tag
    /// </summary>
    Task CreateTagAsync(TagDbo tag);

    #endregion
    
    #region Get

    /// <summary>
    /// Get all tags (filtering by subtype if needed). If subtype == null, then no filters applied
    /// </summary>
    Task<IReadOnlyCollection<TagDbo>> GetTagsAsync(TagSubtype? subtype = null);

    /// <summary>
    /// Get tags, representing categories (ordered by CategoryOrder)
    /// </summary>
    Task<IReadOnlyCollection<TagDbo>> GetCategoryTagsAsync();

    /// <summary>
    /// Get tag by ID. If tag is not found - throws an exception
    /// </summary>
    Task<TagDbo> GetTagByIdAsync(Guid id);
    
    /// <summary>
    /// Get tag by name. Probably will be used only for import from old FT
    /// </summary>
    Task<TagDbo> GetTagByNameAsync(string name);
    
    /// <summary>
    /// Get popularity for given tags
    /// </summary>
    Task<Dictionary<Guid, int>> GetTagsPopularity(IReadOnlyCollection<Guid> tagsIds);

    /// <summary>
    /// Get max texts count from all tags
    /// </summary>
    /// <returns></returns>
    Task<int> GetMaxTextsCountAsync();

    #endregion
}