namespace webapi.Models.Api.Enums;

/// <summary>
/// Special text types
/// </summary>
public enum SpecialTextType
{
    /// <summary>
    /// Ordinary text
    /// </summary>
    Normal = 0,
    
    /// <summary>
    /// Contest text
    /// </summary>
    Contest = 1,
    
    /// <summary>
    /// Poor-quality text
    /// </summary>
    Sandbox = 2,
    
    /// <summary>
    /// Snuff
    /// </summary>
    Snuff = 3
}