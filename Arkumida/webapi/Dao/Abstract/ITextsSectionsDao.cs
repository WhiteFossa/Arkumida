using webapi.Dao.Models;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with text sections
/// </summary>
public interface ITextsSectionsDao
{
    #region Create / Update
    
    /// <summary>
    /// Create text section
    /// </summary>
    Task CreateTextSectionAsync(TextSectionDbo section);

    /// <summary>
    /// Add existing variant to section
    /// </summary>
    Task AddVariantToSection(Guid sectionId, Guid variantId);

    #endregion
}