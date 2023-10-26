using webapi.Dao.Models;
using webapi.Dao.Models.Enums;

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

    /// <summary>
    /// Add file to text
    /// </summary>
    Task<TextFileDbo> AddFileToTextAsync(Guid textId, string name, Guid fileId);
    
    #endregion

    #region Get

    /// <summary>
    /// Get some texts metadata (i.e. actual content is NOT loaded)
    /// </summary>
    Task<IReadOnlyCollection<TextDbo>> GetTextsMetadataAsync(TextOrderMode orderMode, int skip, int take);

    /// <summary>
    /// Get texts metadata for given texts IDs
    /// Ordering is by LastUpdateTime, descending
    /// </summary>
    Task<Dictionary<Guid, TextDbo>> GetTextsMetadataByIdsAsync(IReadOnlyCollection<Guid> textsIds);

    /// <summary>
    /// Get text metadata by text Id. Actual text content is NOT loaded
    /// </summary>
    Task<TextDbo> GetTextMetadataByIdAsync(Guid textId);
    
    /// <summary>
    /// Get total texts count
    /// </summary>
    Task<int> GetTotalTextsCountAsync();

    /// <summary>
    /// Get last text add time
    /// </summary>
    Task<DateTime> GetLastTextAddTimeAsync();

    /// <summary>
    /// Similar to GetTextMetadataByIdAsync(), but do files load
    /// </summary>
    Task<TextDbo> GetTextWithFilesByIdAsync(Guid textId);

    /// <summary>
    /// Load all text files by text
    /// </summary>
    Task<IReadOnlyCollection<TextFileDbo>> GetTextFilesByTextAsync(Guid textId);

    /// <summary>
    /// Get one text page (heavy data like sections and variants returned by this method)
    /// </summary>
    Task<TextPageDbo> GetPageAsync(Guid textId, int pageNumber);

    /// <summary>
    /// Get all pages of text (heavy method, use carefully)
    /// </summary>
    Task<IReadOnlyCollection<TextPageDbo>> GetAllPagesAsync(Guid textId);

    /// <summary>
    /// Get pages count for each of given texts
    /// </summary>
    Task<Dictionary<Guid, int>> GetPagesCountByTexts(IReadOnlyCollection<Guid> textsIds);

    #endregion
}