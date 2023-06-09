using webapi.Dao.Models;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with files
/// </summary>
public interface IFilesDao
{
    #region Create / update

    /// <summary>
    /// Create new file
    /// </summary>
    Task CreateFileAsync(FileDbo file);

    #endregion
    
    #region Get

    /// <summary>
    /// Get file by ID
    /// </summary>
    Task<FileDbo> GetFileAsync(Guid fileId);

    #endregion
}