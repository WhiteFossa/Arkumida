using webapi.Dao.Models;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with texts
/// </summary>
public interface ITextsDao
{
    #region Create / Update
    
    /// <summary>
    /// Create text
    /// </summary>
    Task CreateTextAsync(TextDbo text);

    #endregion
}