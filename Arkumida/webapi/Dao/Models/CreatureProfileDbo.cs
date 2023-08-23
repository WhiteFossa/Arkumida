using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

/// <summary>
/// Arkumida user profile
/// </summary>
public class CreatureProfileDbo
{
    /// <summary>
    /// Profile ID (must match CreatureDbo ID)
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Plaintext password, it is used to send notifications to creatures about "You are registered on Arkumida now, your password is...".
    /// After first successful login it should be wiped-out
    /// </summary>
    public string OneTimePlaintextPassword { get; set; }

    /// <summary>
    /// User's visible name
    /// </summary>
    public string DisplayName { get; set; }
    
    /// <summary>
    /// Creature's avatars
    /// </summary>
    public IList<AvatarDbo> Avatars { get; set; }

    /// <summary>
    /// Creature's current avatar
    /// </summary>
    public AvatarDbo CurrentAvatar { get; set; }
}