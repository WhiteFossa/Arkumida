using webapi.Dao.Models.Enums;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with texts
/// </summary>
public interface ITextsService
{
    /// <summary>
    /// Create new text
    /// </summary>
    Task CreateTextAsync(Text text);

    /// <summary>
    /// Get some texts metadata from database
    /// </summary>
    Task<IReadOnlyCollection<TextInfoDto>> GetTextsMetadataAsync(TextOrderMode orderMode, int skip, int take);

    /// <summary>
    /// Get text metadata by text ID
    /// </summary>
    Task<TextInfoDto> GetTextMetadataByIdAsync(Guid textId);

    /// <summary>
    /// Get total texts count
    /// </summary>
    Task<int> GetTotalTextsCountAsync();

    /// <summary>
    /// Get last text add time
    /// </summary>
    Task<DateTime> GetLastTextAddTimeAsync();

    /// <summary>
    /// Get text data, required to display read page (only one page of text will be displayed at once)
    /// </summary>
    Task<TextReadDto> GetTextToReadAsync(Guid textId, int pageNumber);

    /// <summary>
    /// Order text sections such a way, that they will compose a full text
    /// </summary>
    IReadOnlyCollection<TextSection> OrderTextSections(IEnumerable<TextSection> sections);

    /// <summary>
    /// Parse text (of variant) to text elements
    /// </summary>
    /// <param name="textContent">Plain text to parse</param>
    /// <param name="textFiles">Text files (we need it to generate links to images)</param>
    IReadOnlyCollection<TextElementDto> ParseTextToElements(string textContent, IReadOnlyCollection<TextFile> textFiles);

    /// <summary>
    /// Add existing file to given text under provided name
    /// </summary>
    Task AddFileToTextAsync(Guid textId, string fileName, Guid existingFileId);
}