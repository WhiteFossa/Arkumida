namespace webapi.Models;

/// <summary>
/// Entity with ID
/// </summary>
public class IdedEntity
{
    /// <summary>
    /// Entity ID
    /// </summary>
    public Guid Id { get;  set; }

    /// <summary>
    /// Furry readable ID
    /// </summary>
    public string FurryReadableId { get; set; }
}