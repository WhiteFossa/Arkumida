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
}