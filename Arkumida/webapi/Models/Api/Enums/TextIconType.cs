namespace webapi.Models.Api.Enums;

/// <summary>
/// Additional (because some icons are drawn by frontend itself) icons for texts
/// </summary>
public enum TextIconType
{
    /// <summary>
    /// Text have illustrations
    /// </summary>
    Illustrations = 3,
    
    /// <summary>
    /// Text is incomplete
    /// </summary>
    Incomplete = 4,
    
    /// <summary>
    /// Ponies
    /// </summary>
    MLP = 5,
    
    /// <summary>
    /// The text is a part of series
    /// </summary>
    Series = 6
}