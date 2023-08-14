using webapi.Models;
using webapi.Models.Api.DTOs;

namespace webapi.Services.Abstract;

/// <summary>
/// Utils to work with texts (common code for TextsService, TextsRenderingService and so on)
/// </summary>
public interface ITextUtilsService
{
    /// <summary>
    /// Get raw (i.e. not parsed yet) text from database
    /// </summary>
    Task<string> GetRawTextAsync(Guid textId);
    
    /// <summary>
    /// Parse text (of variant) to text elements
    /// </summary>
    /// <param name="textContent">Plain text to parse</param>
    /// <param name="textFiles">Text files (we need it to generate links to images)</param>
    IReadOnlyCollection<TextElementDto> ParseTextToElements(string textContent, IReadOnlyCollection<TextFile> textFiles);
}