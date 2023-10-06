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

    /// <summary>
    /// Mass get creatures profiles by IDs
    /// </summary>
    Task<IReadOnlyCollection<CreatureProfileDbo>> MassGetProfilesAsync(IReadOnlyCollection<Guid> creaturesIds);

    /// <summary>
    /// Return all creatures profiles, who's display names contain displayNamePart (case insensitive)
    /// </summary>
    Task<IReadOnlyCollection<CreatureProfileDbo>> FindCreaturesProfilesByDisplayNamePartAsync(string displayNamePart);

    /// <summary>
    /// Find creature by display name
    /// </summary>
    Task<CreatureProfileDbo> FindCreatureByDisplayNameAsync(string displayName);
}