namespace webapi.Dao.Models.Enums;

/// <summary>
/// Possible category tag types
/// </summary>
public enum CategoryTagType
{
    /// <summary>
    /// Normal category tag
    /// </summary>
    Normal = 0,
    
    /// <summary>
    /// Snuff category, mark with pink and show warning
    /// </summary>
    Snuff = 1,
    
    /// <summary>
    /// Poor-quality texts, mark with gray and show warning
    /// </summary>
    Sandbox = 2,
    
    /// <summary>
    /// Contest texts, mark with yellow
    /// </summary>
    Contest = 3
}