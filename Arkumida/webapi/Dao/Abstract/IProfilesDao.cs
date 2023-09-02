using webapi.Dao.Models;

namespace webapi.Dao.Abstract;

/// <summary>
/// Interface for work with creatures profiles
/// </summary>
public interface IProfilesDao
{
    /// <summary>
    /// Create a profile
    /// </summary>
    Task<CreatureProfileDbo> CreateProfileAsync(CreatureProfileDbo profile);

    /// <summary>
    /// Update the profile
    /// </summary>
    Task<CreatureProfileDbo> UpdateProfileAsync(CreatureProfileDbo newProfile);
    
    /// <summary>
    /// Get creature profile
    /// </summary>
    Task<CreatureProfileDbo> GetProfileAsync(Guid creatureId);
}