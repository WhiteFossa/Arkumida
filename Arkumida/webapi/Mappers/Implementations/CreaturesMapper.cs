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
            creature.Email
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
            Email = creature.Email
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