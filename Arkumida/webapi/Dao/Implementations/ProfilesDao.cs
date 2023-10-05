using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Dao.Implementations;

public class ProfilesDao : IProfilesDao
{
    private readonly MainDbContext _dbContext;
    private readonly UserManager<CreatureDbo> _userManager;

    public ProfilesDao
    (
        MainDbContext dbContext,
        UserManager<CreatureDbo> userManager
    )
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<CreatureProfileDbo> CreateProfileAsync(CreatureProfileDbo profile)
    {
        _ = profile ?? throw new ArgumentNullException(nameof(profile), "Profile must not be null!");
        
        // Do we have the user with given profile ID?
        var creature = await _userManager.FindByIdAsync(profile.Id.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Attempt to create a profile for non-existing creature. Creature ID={ profile.Id }", nameof(profile.Id));
        }
        
        await _dbContext
            .Profiles
            .AddAsync(profile);

        await _dbContext.SaveChangesAsync();

        return profile;
    }

    public async Task<CreatureProfileDbo> UpdateProfileAsync(CreatureProfileDbo newProfile)
    {
        var profile = await GetProfileAsync(newProfile.Id);

        profile = await LoadLinkedEntities(profile);

        profile.IsPasswordChangeRequired = newProfile.IsPasswordChangeRequired;
        profile.OneTimePlaintextPassword = newProfile.OneTimePlaintextPassword;
        profile.DisplayName = newProfile.DisplayName;
        profile.About = newProfile.About;

        await _dbContext.SaveChangesAsync();

        return profile;
    }

    private async Task<CreatureProfileDbo> LoadLinkedEntities(CreatureProfileDbo profile)
    {
        profile.CurrentAvatar = await LoadLinkedAvatarAsync(profile.CurrentAvatar);

        profile.Avatars = profile
            .Avatars
            .Select(async a => await LoadLinkedAvatarAsync(a))
            .Select(t => t.Result)
            .ToList();
        
        return profile;
    }
    
    private async Task<AvatarDbo> LoadLinkedAvatarAsync(AvatarDbo avatar)
    {
        if (avatar == null)
        {
            return null;
        }

        var loadedAvatar = await _dbContext.Avatars.SingleOrDefaultAsync(a => a.Id == avatar.Id);
        if (loadedAvatar == null)
        {
            // No avatar in DB
            throw new ArgumentException($"Avatar with ID={avatar.Id} is not found in DB!", nameof(avatar.Id));
        }

        return loadedAvatar;
    }
    
    public async Task<CreatureProfileDbo> GetProfileAsync(Guid creatureId)
    {
        return await _dbContext
            .Profiles
                
            .Include(p => p.CurrentAvatar)
            .ThenInclude(ca => ca.File)
            
            .Include(p => p.Avatars)
            .ThenInclude(a => a.File)
            
            .SingleAsync(p => p.Id == creatureId);
    }

    public async Task<IReadOnlyCollection<CreatureProfileDbo>> MassGetProfilesAsync(IReadOnlyCollection<Guid> creaturesIds)
    {
        return await _dbContext
            .Profiles

            .Include(p => p.CurrentAvatar)
            .ThenInclude(ca => ca.File)

            .Include(p => p.Avatars)
            .ThenInclude(a => a.File)

            .Where(p => creaturesIds.Contains(p.Id))

            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<CreatureProfileDbo>> FindCreaturesProfilesByDisplayNamePartAsync(string displayNamePart)
    {
        return await _dbContext
            .Profiles

            .Include(p => p.CurrentAvatar)
            .ThenInclude(ca => ca.File)

            .Include(p => p.Avatars)
            .ThenInclude(a => a.File)

            .Where(p => p.DisplayName.ToLower().Contains(displayNamePart.ToLower()))

            .ToListAsync();
    }
}