using webapi.Dao.Models.Enums.RenderedTexts;
using webapi.Models;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to render texts / get rendered texts
/// </summary>
public interface ITextsRenderingService
{
    /// <summary>
    /// Render text to file of given type
    /// </summary>
    Task<byte[]> RenderTextToFileContentAsync(Text metadata, RenderedTextType type);

    /// <summary>
    /// Get rendered text. Will return null if rendered text doesn't exist (in this case you probably want to render a text and put it to DB)
    /// </summary>
    Task<RenderedText> GetRenderedTextAsync(Guid textId, RenderedTextType type);

    /// <summary>
    /// Renders text (using RenderTextToFileContentAsync()) and puts it into database
    /// </summary>
    Task<RenderedText> RenderTextToDbAsync(Guid textId, RenderedTextType type);
}