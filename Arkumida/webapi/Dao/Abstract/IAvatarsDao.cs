using webapi.Dao.Models;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with avatars
/// </summary>
public interface IAvatarsDao
{
    /// <summary>
    /// Add avatar to given creature's collection
    /// </summary>
    Task AddAvatarToUserAsync(Guid creatureId, AvatarDbo avatar);
}