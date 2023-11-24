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

namespace webapi.Mappers.Implementations;

public class AvatarsMapper : IAvatarsMapper
{
    private readonly IFilesMapper _filesMapper;

    public AvatarsMapper
    (
        IFilesMapper filesMapper
    )
    {
        _filesMapper = filesMapper;
    }
    
    public IReadOnlyCollection<Avatar> Map(IEnumerable<AvatarDbo> avatars)
    {
        if (avatars == null)
        {
            return null;
        }

        return avatars.Select(a => Map(a)).ToList();
    }

    public Avatar Map(AvatarDbo avatar)
    {
        if (avatar == null)
        {
            return null;
        }

        return new Avatar()
        {
            Id = avatar.Id,
            Name = avatar.Name,
            UploadTime = avatar.UploadTime,
            File = _filesMapper.Map(avatar.File) 
        };
    }

    public AvatarDbo Map(Avatar avatar)
    {
        if (avatar == null)
        {
            return null;
        }
        
        return new AvatarDbo()
        {
            Id  = avatar.Id,
            Name = avatar.Name,
            UploadTime = avatar.UploadTime,
            File = _filesMapper.Map(avatar.File)
        };
    }

    public IReadOnlyCollection<AvatarDbo> Map(IEnumerable<Avatar> avatars)
    {
        if (avatars == null)
        {
            return null;
        }

        return avatars.Select(a => Map(a)).ToList();
    }
}