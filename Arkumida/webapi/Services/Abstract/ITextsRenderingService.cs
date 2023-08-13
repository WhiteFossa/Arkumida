using webapi.Dao.Models.Enums.RenderedTexts;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to render texts / get rendered texts
/// </summary>
public interface ITextsRenderingService
{
    /// <summary>
    /// Render text to file of given type
    /// </summary>
    Task<byte[]> RenderTextToFileContent(Guid textId, RenderedTextType type);
}