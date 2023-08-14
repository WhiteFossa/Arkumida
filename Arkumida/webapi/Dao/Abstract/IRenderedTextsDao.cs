using webapi.Dao.Models;
using webapi.Dao.Models.Enums.RenderedTexts;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with rendered texts
/// </summary>
public interface IRenderedTextsDao
{
    #region Create / Update

    /// <summary>
    /// Create rendered text
    /// </summary>
    Task CreateRenderedTextAsync(RenderedTextDbo renderedText);

    #endregion
    
    #region Get

    /// <summary>
    /// Get rendered text (may return null if there is no such rendered text)
    /// </summary>
    Task<RenderedTextDbo> GetRenderedTextAsync(Guid textId, RenderedTextType type);

    #endregion
}