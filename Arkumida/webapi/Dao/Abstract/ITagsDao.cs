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

    #endregion
}