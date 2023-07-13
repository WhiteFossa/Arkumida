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
    /// Similar to GetTextMetadataByIdAsync(), but do full load (i.e. sections, variants and other heavy data, required to display text)
    /// </summary>
    Task<TextDbo> GetTextByIdAsync(Guid textId);
    
    #endregion
}