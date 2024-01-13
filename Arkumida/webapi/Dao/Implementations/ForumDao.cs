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
using webapi.Dao.Models.Forum;

namespace webapi.Dao.Implementations;

public class ForumDao : IForumDao
{
    private readonly MainDbContext _dbContext;

    public ForumDao
    (
        MainDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }
    
    public async Task<ForumSectionDbo> CreateSectionAsync(ForumSectionDbo sectionDbo)
    {
        _ = sectionDbo ?? throw new ArgumentNullException(nameof(sectionDbo), "Forum section must not be null!");

        sectionDbo.Author = await _dbContext.Users.SingleAsync(c => c.Id == sectionDbo.Author.Id);

        for (var i = 0; i < sectionDbo.Subsections.Count; i++)
        {
            sectionDbo.Subsections[i] = await _dbContext.ForumSections.SingleAsync(fs => fs.Id == sectionDbo.Subsections[i].Id);
        }

        for (var i = 0; i < sectionDbo.Topics.Count; i++)
        {
            sectionDbo.Topics[i] = await _dbContext.ForumTopics.SingleAsync(ft => ft.Id == sectionDbo.Topics[i].Id);
        }

        await _dbContext
            .ForumSections
            .AddAsync(sectionDbo);

        return sectionDbo;
    }

    public async Task<ForumSectionDbo> GetSectionByNameAsync(string name)
    {
        return await _dbContext
            .ForumSections
            .SingleOrDefaultAsync(fs => fs.Name == name);
    }
}