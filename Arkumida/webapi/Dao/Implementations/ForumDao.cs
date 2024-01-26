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

using System.Runtime.InteropServices.JavaScript;
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

        await _dbContext.SaveChangesAsync();

        return sectionDbo;
    }

    public async Task UpdateSectionAsync(ForumSectionDbo newSection)
    {
        _ = newSection ?? throw new ArgumentNullException(nameof(newSection), "Section mustn't be null!");

        var section = await GetSectionByIdAsync(newSection.Id);

        section.Name = newSection.Name;
        section.Description = newSection.Description;
        section.CreationTime = newSection.CreationTime;
        section.Author = await _dbContext.Users.SingleAsync(c => c.Id == newSection.Author.Id);

        var newSubsectionsIds = newSection
            .Subsections
            .Select(fs => fs.Id);
        
        section.Subsections = new List<ForumSectionDbo>();
        foreach (var subsectionId in newSubsectionsIds)
        {
            section.Subsections.Add(await _dbContext.ForumSections.SingleAsync(fs => fs.Id == subsectionId));
        }

        var newTopicIds = newSection
            .Topics
            .Select(ft => ft.Id);
        
        section.Topics = new List<ForumTopicDbo>();
        foreach (var topicId in newTopicIds)
        {
            section.Topics.Add(await _dbContext.ForumTopics.SingleAsync(ft => ft.Id == topicId));
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<ForumTopicDbo> CreateTopicAsync(ForumTopicDbo topicDbo, Guid sectionId)
    {
        _ = topicDbo ?? throw new ArgumentNullException(nameof(topicDbo), "Forum topic must not be null!");

        var section = await GetSectionByIdAsync(sectionId);
        if (section == null)
        {
            throw new ArgumentException($"Section with ID={sectionId} is not found!", nameof(sectionId));
        }
        
        for (var i = 0; i < topicDbo.Messages.Count; i++)
        {
            topicDbo.Messages[i] = await _dbContext.ForumMessages.SingleAsync(fm => fm.Id == topicDbo.Messages[i].Id);
        }

        topicDbo.CommentsForText = topicDbo.CommentsForText != null ?  await _dbContext.Texts.SingleAsync(t => t.Id == topicDbo.CommentsForText.Id) : null;
        
        await _dbContext
            .ForumTopics
            .AddAsync(topicDbo);

        await _dbContext.SaveChangesAsync();
        
        section.Topics.Add(topicDbo);
        await UpdateSectionAsync(section);
        
        return topicDbo;
    }

    public async Task<ForumSectionDbo> GetSectionByIdAsync(Guid id)
    {
        return await _dbContext
            .ForumSections
            .Include(fs => fs.Author)
            .Include(fs => fs.Subsections)
            .Include(fs => fs.Topics)
            .SingleOrDefaultAsync(fs => fs.Id == id);
    }

    public async Task<ForumSectionDbo> GetSectionByNameAsync(string name)
    {
        return await _dbContext
            .ForumSections
            .Include(fs => fs.Author)
            .Include(fs => fs.Subsections)
            .Include(fs => fs.Topics)
            .SingleOrDefaultAsync(fs => fs.Name == name);
    }

    public async Task<ForumTopicDbo> GetTopicByIdAsync(Guid id)
    {
        return await _dbContext
            .ForumTopics
                
            .Include(ft => ft.Messages)
            .ThenInclude(fm => fm.Author)
            
            .Include(ft => ft.CommentsForText)
            
            .SingleOrDefaultAsync(ft => ft.Id == id);
    }

    public async Task<ForumTopicDbo> GetTextCommentsTopicByTextId(Guid textId)
    {
        return await _dbContext
            .ForumTopics
                
            .Include(ft => ft.Messages)
            .ThenInclude(fm => fm.Author)
            
            .Include(ft => ft.CommentsForText)
            
            .SingleOrDefaultAsync(ft => ft.CommentsForText.Id == textId);
    }

    public async Task<bool> IsTopicExistInSectionAsync(Guid sectionId, string topicName)
    {
        return await _dbContext
            .ForumSections
            .Where(fs => fs.Id == sectionId)
            .Where(fs => fs.Topics.Select(ft => ft.Name).Contains(topicName))
            .AnyAsync();




    }
}