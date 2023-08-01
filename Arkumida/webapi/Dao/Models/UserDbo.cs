using Microsoft.AspNetCore.Identity;

namespace webapi.Dao.Models;

/// <summary>
/// Arkumida user (users, authors, translators etc)
/// </summary>
public class UserDbo : IdentityUser<Guid>
{
    /// <summary>
    /// User's visible name
    /// </summary>
    public string DisplayName { get; set; }
}