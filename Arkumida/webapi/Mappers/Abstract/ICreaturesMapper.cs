using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper to map users
/// </summary>
public interface ICreaturesMapper
{
    IReadOnlyCollection<Creature> Map(IEnumerable<CreatureDbo> creatures);

    Creature Map(CreatureDbo creature);

    CreatureDbo Map(Creature creature);

    IReadOnlyCollection<CreatureDbo> Map(IEnumerable<Creature> creatures);
}