using webapi.Models;
using webapi.Models.Api.DTOs;

namespace webapi.Services.Abstract.TextRenderers;

/// <summary>
/// Raw text renderer (like Plain Text Renderer, but without header)
/// </summary>
public interface IRawTextRenderer
{
    /// <summary>
    /// Render raw text to string
    /// </summary>
    Task<string> RenderAsync(Text textMetadata, IReadOnlyCollection<TextElementDto> textElements);
}