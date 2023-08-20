using Microsoft.AspNetCore.Identity;

namespace webapi.Dao.Models;

/// <summary>
/// Arkumida user (users, authors, translators etc)
/// </summary>
public class CreatureDbo : IdentityUser<Guid>
{
    #region Relations

    /// <summary>
    /// This creature is author of the next texts
    /// </summary>
    public IList<TextDbo> TextsAuthor { get; set; }
    
    /// <summary>
    /// This creature is translator of the next texts
    /// </summary>
    public IList<TextDbo> TextsTranslator { get; set; }

    #endregion

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
}