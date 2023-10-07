using Microsoft.AspNetCore.Identity;

namespace webapi.Dao.Models;

/// <summary>
/// Arkumida user (users, authors, translators etc)
/// </summary>
public class CreatureDbo : IdentityUser<Guid>
{
    /// <summary>
    /// This creature is author of the next texts
    /// </summary>
    public IList<TextDbo> TextsAuthor { get; set; }
    
    /// <summary>
    /// This creature is translator of the next texts
    /// </summary>
    public IList<TextDbo> TextsTranslator { get; set; }
    
    /// <summary>
    /// The creature is author for the next private messages
    /// </summary>
    public IList<PrivateMessageDbo> SenderOfThisPrivateMessages { get; set; }
    
    /// <summary>
    /// The creature is receiver of the next private messages
    /// </summary>
    public IList<PrivateMessageDbo> ReceiverOfThisPrivateMessages { get; set; }
}