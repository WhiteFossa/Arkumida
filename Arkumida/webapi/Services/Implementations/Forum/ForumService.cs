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

using Microsoft.AspNetCore.Identity;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Forum;
using webapi.Mappers.Abstract;
using webapi.Models.Forum;
using webapi.Services.Abstract.Forum;

namespace webapi.Services.Implementations.Forum;

public class ForumService : IForumService
{
    private readonly IForumDao _forumDao;
    private readonly UserManager<CreatureDbo> _userManager;
    private readonly IForumMapper _forumMapper;

    public ForumService
    (
        IForumDao forumDao,
        UserManager<CreatureDbo> userManager,
        IForumMapper forumMapper
    )
    {
        _forumDao = forumDao;
        _userManager = userManager;
        _forumMapper = forumMapper;
    }
    
    public async Task<ForumSection> CreateSectionAsync(string name, string description, Guid authorId, Guid? id = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name mustn't be empty!", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description mustn't be empty!", nameof(description));
        }
        
        // Sections names have to be unique
        if (await _forumDao.GetSectionByNameAsync(name) != null)
        {
            throw new ArgumentException($"The section with name { name } already exist!", nameof(name));
        }

        var authorDbo = await _userManager.FindByIdAsync(authorId.ToString());
        if (authorDbo == null)
        {
            throw new ArgumentException($"Author creature with ID={authorId} not found!", nameof(authorId));
        }
        
        var sectionDbo = new ForumSectionDbo()
        {
            Id = id ?? Guid.Empty,
            Name = name,
            Description = description,
            CreationTime = DateTime.UtcNow,
            Author = authorDbo,
            Subsections = new List<ForumSectionDbo>(),
            Topics = new List<ForumTopicDbo>()
        };

        return await _forumMapper.MapAsync(await _forumDao.CreateSectionAsync(sectionDbo));
    }
}