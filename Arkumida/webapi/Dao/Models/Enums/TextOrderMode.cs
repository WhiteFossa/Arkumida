namespace webapi.Dao.Models.Enums;

/// <summary>
/// Feed this mode to GetTextsAsync() in ITextsService
/// </summary>
public enum TextOrderMode
{
    /// <summary>
    /// Order by time
    /// </summary>
    Latest = 0,
    
    /// <summary>
    /// Order by popularity
    /// </summary>
    Popular = 1
}