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
    /// Get text data, required to display read page
    /// </summary>
    Task<TextReadDto> GetTextToReadAsync(Guid textId);
}