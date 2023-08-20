using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

/// <summary>
/// User's avatar
/// </summary>
public class AvatarDbo
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Is current avatar for given creature? (Only one avatar can be current)
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Upload time (for ordering)
    /// </summary>
    public DateTime UploadTime { get; set; }

    /// <summary>
    /// File with avatar
    /// </summary>
    public FileDbo File { get; set; }
    
    /// <summary>
    /// Avatar belong to this creature
    /// </summary>
    public CreatureDbo Creature { get; set; }
}