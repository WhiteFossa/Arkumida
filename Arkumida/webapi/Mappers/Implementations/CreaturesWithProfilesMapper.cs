using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class CreaturesWithProfilesMapper : ICreaturesWithProfilesMapper
{
    private readonly IAvatarsMapper _avatarsMapper;

    public CreaturesWithProfilesMapper
    (
        IAvatarsMapper avatarsMapper
    )
    {
        _avatarsMapper = avatarsMapper;
    }
    
    public CreatureWithProfile Map(CreatureDbo creature, CreatureProfileDbo profile)
    {
        return new CreatureWithProfile
        (
            creature.Id,
            creature.UserName,
            creature.Email,
            profile.DisplayName,
            _avatarsMapper.Map(profile.Avatars).ToList(),
            _avatarsMapper.Map(profile.CurrentAvatar),
            profile.About
        );
    }

    public Tuple<CreatureDbo, CreatureProfileDbo> Map(CreatureWithProfile creatureWithProfile)
    {
        var creatureDbo = new CreatureDbo()
        {
            Id = creatureWithProfile.Id,
            UserName = creatureWithProfile.Login,
            Email = creatureWithProfile.Email
        };

        var profileDbo = new CreatureProfileDbo()
        {
            Id = creatureWithProfile.Id,
            DisplayName = creatureWithProfile.DisplayName,
            OneTimePlaintextPassword = string.Empty,
            Avatars = _avatarsMapper.Map(creatureWithProfile.Avatars).ToList(),
            CurrentAvatar = _avatarsMapper.Map(creatureWithProfile.CurrentAvatar),
            About = creatureWithProfile.About
        };

        return new Tuple<CreatureDbo, CreatureProfileDbo>(creatureDbo, profileDbo);
    }
}