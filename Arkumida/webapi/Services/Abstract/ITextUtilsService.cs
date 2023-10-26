using webapi.Dao.Models;
using webapi.Dao.Models.Enums;
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
    
    /// <summary>
    /// Get full text metadata (i.e. with publisher, translators and so on) by text ID
    /// </summary>
    Task<Text> GetTextMetadataAsync(Guid textId);
    
    /// <summary>
    /// Get full text metadata for multiple texts
    /// </summary>
    Task<IReadOnlyCollection<Text>> GetTextsMetadatasAsync(TextOrderMode orderMode, int skip, int take);

    /// <summary>
    /// Load-in some data into text metadata (like full data on authors/translators/publisher)
    /// </summary>
    Task<Text> PopulateTextMetadataAsync(TextDbo metadata);
}