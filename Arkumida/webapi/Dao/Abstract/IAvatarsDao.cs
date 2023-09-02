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

    /// <summary>
    /// Update avatar
    /// </summary>
    Task<AvatarDbo> UpdateAvatarAsync(AvatarDbo avatarToUpdate);

    /// <summary>
    /// Delete avatar
    /// </summary>
    Task DeleteAvatarAsync(Guid avatarId);

    /// <summary>
    /// Get avatar by Id (may return null in case of incorrect avatar)
    /// </summary>
    Task<AvatarDbo> GetAvatarByIdAsync(Guid avatarId);
}