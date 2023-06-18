using webapi.Models;

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
    /// Add existing section to text
    /// </summary>
    Task AddSectionToTextAsync(Guid textId, Guid sectionId);
}