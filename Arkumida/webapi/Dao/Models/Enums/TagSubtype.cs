namespace webapi.Dao.Models.Enums;

/// <summary>
/// Subtype for tags (type of action, species and so on)
/// </summary>
public enum TagSubtype
{
    /// <summary>
    /// Participants, e.g. M/F
    /// </summary>
    Participants = 1,
    
    /// <summary>
    /// Species, e.g. earlybeast
    /// </summary>
    Species = 2,
    
    /// <summary>
    /// Setting, e.g. SciFi
    /// </summary>
    Setting = 3,
    
    /// <summary>
    /// Actions, e.g. erotic tickling
    /// </summary>
    Actions = 4
}