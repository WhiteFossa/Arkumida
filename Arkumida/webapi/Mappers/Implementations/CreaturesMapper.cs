using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class CreaturesMapper : ICreaturesMapper
{
    public IReadOnlyCollection<Creature> Map(IEnumerable<CreatureDbo> creatures)
    {
        if (creatures == null)
        {
            return null;
        }

        return creatures.Select(u => Map(u)).ToList();
    }

    public Creature Map(CreatureDbo creature)
    {
        if (creature == null)
        {
            return null;
        }
        
        return new Creature
        (
            creature.Id,
            creature.UserName,
            creature.Email,
            creature.DisplayName
        );
    }

    public CreatureDbo Map(Creature creature)
    {
        if (creature == null)
        {
            return null;
        }
        
        return new CreatureDbo()
        {
            Id = creature.Id,
            UserName = creature.Login,
            Email = creature.Email,
            DisplayName = creature.DisplayName
        };
    }

    public IReadOnlyCollection<CreatureDbo> Map(IEnumerable<Creature> creatures)
    {
        if (creatures == null)
        {
            return null;
        }

        return creatures.Select(u => Map(u)).ToList();
    }
}