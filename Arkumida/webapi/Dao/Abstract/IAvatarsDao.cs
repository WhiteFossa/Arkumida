using webapi.Dao.Models;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with avatars
/// </summary>
public interface IAvatarsDao
{
    /// <summary>
    /// Creates avatar and adds it to creature's collection
    /// </summary>
    Task<AvatarDbo> AddAvatarToCreatureAsync(Guid creatureId, AvatarDbo avatar);
}