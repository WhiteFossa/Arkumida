using webapi.Models;
using webapi.Models.Api.DTOs;

namespace webapi.Services.Abstract.TextRenderers;

/// <summary>
/// Renders raw text to plain text (txt file), ready to download by user
/// </summary>
public interface IPlainTextRenderer
{
    /// <summary>
    /// Render raw text to txt file
    /// </summary>
    Task<string> RenderAsync(Text textMetadata, IReadOnlyCollection<TextElementDto> textElements);
}