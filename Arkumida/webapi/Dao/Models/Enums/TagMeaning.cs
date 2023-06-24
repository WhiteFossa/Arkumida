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
    TypeStories = 1,
    
    /// <summary>
    /// This tag marks novels category
    /// </summary>
    TypeNovels = 2,
    
    /// <summary>
    /// This tag marks poetry category
    /// </summary>
    TypePoetry = 3,
    
    /// <summary>
    /// This tag marks comics category
    /// </summary>
    TypeComics = 4,

    /// <summary>
    /// Contest text (special type)
    /// </summary>
    SpecTypeContest = 5,
    
    /// <summary>
    /// Poor-quality text (special type)
    /// </summary>
    SpecTypeSandbox = 6,
    
    /// <summary>
    /// Snuff (special type)
    /// </summary>
    SpecTypeSnuff = 7
}