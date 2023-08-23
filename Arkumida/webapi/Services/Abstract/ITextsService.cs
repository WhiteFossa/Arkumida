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
    /// Create new text. Returns created text, which is different from input one (for example it have ID and users populated)
    /// </summary>
    Task<Text> CreateTextAsync(Text text);

    /// <summary>
    /// Get some texts metadata from database
    /// </summary>
    Task<IReadOnlyCollection<TextInfoDto>> GetTextsInfosAsync(TextOrderMode orderMode, int skip, int take);

    /// <summary>
    /// Get text metadata by text ID
    /// </summary>
    Task<TextInfoDto> GetTextInfoByIdAsync(Guid textId);

    /// <summary>
    /// Get total texts count
    /// </summary>
    Task<int> GetTotalTextsCountAsync();

    /// <summary>
    /// Get last text add time
    /// </summary>
    Task<DateTime> GetLastTextAddTimeAsync();

    /// <summary>
    /// Get text metadata, required to display read page (actual text is NOT returned, for text see GetTextPageAsync()) 
    /// </summary>
    Task<TextReadDto> GetTextToReadAsync(Guid textId);
    
    /// <summary>
    /// Get text page data (actual text returned here)
    /// </summary>
    Task<TextPageDto> GetTextPageAsync(Guid textId, int pageNumber);

    /// <summary>
    /// Order text sections such a way, that they will compose a full text
    /// </summary>
    IReadOnlyCollection<TextSection> OrderTextSections(IEnumerable<TextSection> sections);

    /// <summary>
    /// Add existing file to given text under provided name
    /// </summary>
    Task AddFileToTextAsync(Guid textId, string fileName, Guid existingFileId);
}