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
using Microsoft.Extensions.Options;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Forum;
using webapi.Mappers.Abstract;
using webapi.Models.Forum;
using webapi.Models.Settings;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Forum;

namespace webapi.Services.Implementations.Forum;

public class ForumService : IForumService
{
    private readonly IForumDao _forumDao;
    private readonly UserManager<CreatureDbo> _userManager;
    private readonly IForumMapper _forumMapper;
    private readonly ITextUtilsService _textUtilsService;
    private readonly ITextsDao _textsDao;
    private readonly ForumSettings _forumSettings;

    public ForumService
    (
        IForumDao forumDao,
        UserManager<CreatureDbo> userManager,
        IForumMapper forumMapper,
        ITextUtilsService textUtilsService,
        ITextsDao textsDao,
        IOptions<ForumSettings> forumSettings
    )
    {
        _forumDao = forumDao;
        _userManager = userManager;
        _forumMapper = forumMapper;
        _textUtilsService = textUtilsService;
        _textsDao = textsDao;
        _forumSettings = forumSettings.Value;
    }
    
    public async Task<ForumSection> CreateSectionAsync(string name, string description, Guid authorId, Guid? id = null)
    {
        // Title must be filled, description may be empty
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name mustn't be empty!", nameof(name));
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

    public async Task<ForumSection> GetSectionByIdAsync(Guid id)
    {
        return await _forumMapper.MapAsync(await _forumDao.GetSectionByIdAsync(id));
    }

    public async Task<ForumSection> GetSectionByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name mustn't be empty!", nameof(name));
        }

        return await _forumMapper.MapAsync(await _forumDao.GetSectionByNameAsync(name));
    }

    public async Task<ForumTopic> GetTopicByIdAsync(Guid id)
    {
        return await _forumMapper.MapAsync(await _forumDao.GetTopicByIdAsync(id));
    }

    public async Task<ForumTopic> GetTextCommentsTopicByTextIdAsync(Guid textId)
    {
        return await _forumMapper.MapAsync(await _forumDao.GetTextCommentsTopicByTextId(textId));
    }

    public async Task<ForumTopic> CreateTopicAsync
    (
        string name,
        string description,
        Guid sectionId,
        Guid? textId = null
    )
    {
        // Name must be filled, description may be empty
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name mustn't be empty!", nameof(name));
        }
        
        // TODO: Maybe we will uncomment it in future. For now we have some texts with the same name
        /*// Topic name must be unique within section
        if (await _forumDao.IsTopicExistInSectionAsync(sectionId, name))
        {
            throw new ArgumentException("Topic name must be unique within section!", nameof(name));
        }*/
        
        var textDbo = textId.HasValue ? await _textsDao.GetTextMetadataByIdAsync(textId.Value): null;
        if (textId.HasValue && textDbo == null)
        {
            throw new ArgumentException($"Text-related topic is being created, however TextId={textId} is incorrect!", nameof(textId));
        }

        var topicDbo = new ForumTopicDbo()
        {
            Name = name,
            Description = description,
            Messages = new List<ForumMessageDbo>(),
            CommentsForText = textDbo
        };

        return await _forumMapper.MapAsync(await _forumDao.CreateTopicAsync(topicDbo, sectionId));
    }

    public async Task<IReadOnlyCollection<ForumMessage>> GetLastMessagesInTopicAsync(Guid topicId, int skip, int take)
    {
        return await _forumMapper.MapAsync(await _forumDao.GetLastMessagesInTopicAsync(topicId, skip, take));
    }

    public async Task<int> GetTopicMessagesCountAsync(Guid topicId)
    {
        return await _forumDao.GetTopicMessagesCountAsync(topicId);
    }

    public async Task<ForumMessage> AddTextCommentAsync(Guid textId, ForumMessage messageToAdd)
    {
        var textMetadata = await _textUtilsService.GetTextMetadataAsync(textId);
        if (textMetadata == null)
        {
            throw new ArgumentException($"Text with ID={textId} is not found!", nameof(textId));
        }

        var textCommentsTopic = await GetTextCommentsTopicByTextIdAsync(textId);
        if (textCommentsTopic == null)
        {
            // Topic not found, creating it
            textCommentsTopic = await CreateTopicAsync
            (
                string.Format(_forumSettings.TextCommentsTopicNameTemplate, textMetadata.Title),
                string.Format(_forumSettings.TextCommentsTopicDescriptionTemplate, textMetadata.Title),
                _forumSettings.TextsCommentsSectionId,
                textId
            );
        }

        return await _forumMapper.MapAsync(await _forumDao.CreateMessageAsync(_forumMapper.Map(messageToAdd), textCommentsTopic.Id));
    }

    public async Task<IReadOnlyCollection<ForumMessage>> GetLastCommentsByTextAsync(Guid textId, int skip, int take)
    {
        var textCommentsTopic = await GetTextCommentsTopicByTextIdAsync(textId);
        if (textCommentsTopic == null)
        {
            // No topic - no comments
            return new List<ForumMessage>();
        }

        return await GetLastMessagesInTopicAsync(textCommentsTopic.Id, skip, take);
    }

    public async Task<int> GetTextCommentsCountAsync(Guid textId)
    {
        var textCommentsTopic = await GetTextCommentsTopicByTextIdAsync(textId);
        if (textCommentsTopic == null)
        {
            return 0;
        }
        
        return await GetTopicMessagesCountAsync(textCommentsTopic.Id);
    }
}