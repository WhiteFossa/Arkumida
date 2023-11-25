#region License
// Arkumida - Furtails.pw next generation backend
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Creatures;

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
            profile.IsPasswordChangeRequired,
            profile.OneTimePlaintextPassword,
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
            IsPasswordChangeRequired = creatureWithProfile.IsPasswordChangeRequired,
            OneTimePlaintextPassword = creatureWithProfile.OneTimePlaintextPassword,
            DisplayName = creatureWithProfile.DisplayName,
            Avatars = _avatarsMapper.Map(creatureWithProfile.Avatars).ToList(),
            CurrentAvatar = _avatarsMapper.Map(creatureWithProfile.CurrentAvatar),
            About = creatureWithProfile.About
        };

        return new Tuple<CreatureDbo, CreatureProfileDbo>(creatureDbo, profileDbo);
    }
}