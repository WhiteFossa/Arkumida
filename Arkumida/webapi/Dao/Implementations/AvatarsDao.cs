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

using Microsoft.EntityFrameworkCore;
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

        avatar.File = await _dbContext.Files.SingleAsync(f => f.Id == avatar.File.Id);
        avatar.UploadTime = DateTime.UtcNow;
        
        await _dbContext
            .Avatars
            .AddAsync(avatar);

        await _dbContext.SaveChangesAsync();

        var creature = await _profilesDao.GetProfileAsync(creatureId);
        creature.Avatars.Add(avatar);
        await _profilesDao.UpdateProfileAsync(creature);

        return avatar;
    }

    public async Task<AvatarDbo> UpdateAvatarAsync(AvatarDbo avatarToUpdate)
    {
        _ = avatarToUpdate ?? throw new ArgumentNullException(nameof(avatarToUpdate), "Avatar can't be null!");

        var avatar = await _dbContext
            .Avatars
            .SingleAsync(a => a.Id == avatarToUpdate.Id);

        avatar.Name = avatarToUpdate.Name;
        avatar.UploadTime = avatarToUpdate.UploadTime;
        avatar.File = await _dbContext.Files.SingleAsync(f => f.Id == avatarToUpdate.File.Id);

        await _dbContext.SaveChangesAsync();

        return avatar;
    }

    public async Task DeleteAvatarAsync(Guid avatarId)
    {
        var avatar = await _dbContext
            .Avatars
            .SingleAsync(a => a.Id == avatarId);

        _dbContext.Remove(avatar);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<AvatarDbo> GetAvatarByIdAsync(Guid avatarId)
    {
        return await _dbContext
            .Avatars
            .Include(a => a.File)
            .SingleOrDefaultAsync(a => a.Id == avatarId);
    }
}