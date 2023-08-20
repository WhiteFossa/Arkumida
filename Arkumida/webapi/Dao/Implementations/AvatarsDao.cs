using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;

namespace webapi.Dao.Implementations;

public class AvatarsDao : IAvatarsDao
{
    private readonly MainDbContext _dbContext;
    private readonly UserManager<CreatureDbo> _userManager;

    public AvatarsDao
    (
        MainDbContext dbContext,
        UserManager<CreatureDbo> userManager
    )
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    
    public async Task AddAvatarToUserAsync(Guid creatureId, AvatarDbo avatar)
    {
        _ = avatar ?? throw new ArgumentNullException(nameof(avatar), "Avatar must not be null!");

        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException("Creature with given ID is not found!", nameof(creatureId));
        }
        
        avatar.UploadTime = DateTime.UtcNow;
        avatar.File = await _dbContext.Files.SingleAsync(f => f.Id == avatar.File.Id);

        if (creature.Avatars == null)
        {
            // Creature have no avatars yet
            creature.Avatars = new List<AvatarDbo>();
        }
        
        creature.Avatars.Add(avatar);

        var result = await _userManager.UpdateAsync(creature);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to add avatar to creature with ID={creatureId}!");
        }
    }
}