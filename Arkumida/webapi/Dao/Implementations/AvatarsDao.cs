using webapi.Dao.Abstract;
using webapi.Dao.Models;

namespace webapi.Dao.Implementations;

public class AvatarsDao : IAvatarsDao
{
    private readonly MainDbContext _dbContext;
    private readonly IProfilesDao _profilesDao;

    public AvatarsDao
    (
        MainDbContext dbContext,
        IProfilesDao profilesDao
    )
    {
        _dbContext = dbContext;
        _profilesDao = profilesDao;
    }
    
    public async Task<AvatarDbo> AddAvatarToCreatureAsync(Guid creatureId, AvatarDbo avatar)
    {
        _ = avatar ?? throw new ArgumentNullException(nameof(avatar), "Avatar can't be null!");
        
        await _dbContext
            .Avatars
            .AddAsync(avatar);

        var creature = await _profilesDao.GetProfileAsync(creatureId);
        creature.Avatars.Add(avatar);
        await _profilesDao.UpdateProfileAsync(creature);

        return avatar;
    }
}