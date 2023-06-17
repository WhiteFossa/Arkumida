using webapi.Dao.Models;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with text sections variants
/// </summary>
public interface ITextsSectionsVariantsDao
{
    #region Create / Update

    /// <summary>
    /// Create text section variant
    /// </summary>
    Task CreateTextSectionVariantAsync(TextSectionVariantDbo variant);

    #endregion
}