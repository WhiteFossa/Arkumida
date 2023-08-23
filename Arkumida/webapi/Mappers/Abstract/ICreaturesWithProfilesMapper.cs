using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mappers for creatures with profiles
/// </summary>
public interface ICreaturesWithProfilesMapper
{
    CreatureWithProfile Map(CreatureDbo creature, CreatureProfileDbo profile);

    Tuple<CreatureDbo, CreatureProfileDbo> Map(CreatureWithProfile creatureWithProfile);
}