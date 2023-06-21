namespace webapi.Dao.Models.Enums;

/// <summary>
/// Machine-readable tag meaning
/// </summary>
public enum TagMeaning
{
    /// <summary>
    /// We can't tell anything specific about this tag
    /// </summary>
    Unspecified = 0,
    
    /// <summary>
    /// This tag marks stories category
    /// </summary>
    Stories = 1,
    
    /// <summary>
    /// This tag marks novels category
    /// </summary>
    Novels = 2,
    
    /// <summary>
    /// This tag marks poetry category
    /// </summary>
    Poetry = 3,
    
    /// <summary>
    /// This tag marks comics category
    /// </summary>
    Comics = 4
}