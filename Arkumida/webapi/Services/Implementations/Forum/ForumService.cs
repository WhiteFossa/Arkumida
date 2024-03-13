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
using webapi.Models.Api.Responses.Forum;
using webapi.Models.Forum;
using webapi.Models.Forum.Infos;
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
    private readonly IAccountsService _accountsService;

    public ForumService
    (
        IForumDao forumDao,
        UserManager<CreatureDbo> userManager,
        IForumMapper forumMapper,
        ITextUtilsService textUtilsService,
        ITextsDao textsDao,
        IOptions<ForumSettings> forumSettings,
        IAccountsService accountsService
    )
    {
        _forumDao = forumDao;
        _userManager = userManager;
        _forumMapper = forumMapper;
        _textUtilsService = textUtilsService;
        _textsDao = textsDao;
        _forumSettings = forumSettings.Value;
        _accountsService = accountsService;
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

    public async Task<ForumTopicInfo> GetTopicInfoAsync(Guid topicId)
    {
        var topic = await _forumDao.GetTopicWithoutMessagesByIdAsync(topicId);

        if (topic == null)
        {
            return null;
        }

        return new ForumTopicInfo()
        {
            Id = topic.Id,
            Name = topic.Name,
            Description = topic.Description,
            MessagesCount = (await _forumDao.GetMessagesCountsByTopicsIdsAsync(new [] { topicId })).Values.Single(),
            FirstMessage = await _forumMapper.MapAsync(await _forumDao.GetFirstMessageInTopicAsync(topicId)),
            LastMessage = await _forumMapper.MapAsync(await _forumDao.GetLastMessageInTopicAsync(topicId)),
            CommentsForText = topic.CommentsForText?.Id
        };
    }

    public async Task<Dictionary<Guid, int>> GetMessagesCountsByTopicsIdsAsync(IReadOnlyCollection<Guid> topicsIds)
    {
        return await _forumDao.GetMessagesCountsByTopicsIdsAsync(topicsIds);
    }

    public async Task<IDictionary<Guid, Guid?>> GetTextsTopicsIdsByTextsIdsAsync(IReadOnlyCollection<Guid> textsIds)
    {
        return await _forumDao.GetTextsTopicsIdsByTextsIds(textsIds);
    }

    public async Task<IReadOnlyCollection<ForumMessage>> GetLastMessagesInTopicAsync(Guid topicId, int skip, int take)
    {
        return await _forumMapper.MapAsync(await _forumDao.GetLastMessagesInTopicAsync(topicId, skip, take));
    }

    public async Task<ForumMessage> AddMessageAsync(Guid topicId, Guid authorId, Guid? replyTo, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message must have some content!", nameof(message));
        }
        
        var topic = await _forumDao.GetTopicByIdAsync(topicId);
        if (topic == null)
        {
            throw new ArgumentException($"Topic with ID={ topicId } doesn't exist!", nameof(topicId));
        }

        var author = await _accountsService.GetProfileByCreatureIdAsync(authorId);
        if (author == null)
        {
            throw new ArgumentException($"Creature with ID={ authorId } doesn't exist!", nameof(authorId));
        }

        ForumMessage replyToMessage = null;
        if (replyTo.HasValue)
        {
            replyToMessage = await _forumMapper.MapAsync(await _forumDao.GetMessageByIdAsync(replyTo.Value));

            if (replyToMessage.TopicId != topicId)
            {
                throw new ArgumentException($"Attempt to reply to a message (ID = { replyTo.Value }) in a different topic!", nameof(replyTo));
            }
        }
        
        var currentTime = DateTime.UtcNow;
        
        var messageToAdd = new ForumMessage()
        {
            Author = author,
            ReplyTo = replyToMessage,
            PostTime = currentTime,
            LastUpdateTime = currentTime,
            Message = message
        };
        
        return await _forumMapper.MapAsync(await _forumDao.CreateMessageAsync(_forumMapper.Map(messageToAdd), topicId));
    }

    public async Task<ForumMessage> ImportTextCommentAsync(Guid textId, ForumMessage messageToAdd)
    {
        var textMetadata = await _textUtilsService.GetTextMetadataAsync(textId);
        if (textMetadata == null)
        {
            throw new ArgumentException($"Text with ID={textId} is not found!", nameof(textId));
        }

        var textCommentsTopic = await GetOrCreateTextCommentsTopic(textId, textMetadata.Title);

        return await _forumMapper.MapAsync(await _forumDao.CreateMessageAsync(_forumMapper.Map(messageToAdd), textCommentsTopic.Id));
    }

    public async Task<ForumMessage> AddTextCommentAsync(Guid textId, Guid authorId, Guid? replyTo, string message)
    {
        var textMetadata = await _textUtilsService.GetTextMetadataAsync(textId);
        if (textMetadata == null)
        {
            throw new ArgumentException($"Text with ID={textId} is not found!", nameof(textId));
        }

        var textCommentsTopic = await GetOrCreateTextCommentsTopic(textId, textMetadata.Title);

        return await AddMessageAsync(textCommentsTopic.Id, authorId, replyTo, message);
    }

    private async Task<ForumTopic> GetOrCreateTextCommentsTopic(Guid textId, string textTitle)
    {
        var textCommentsTopic = await GetTextCommentsTopicByTextIdAsync(textId);
        if (textCommentsTopic == null)
        {
            // Topic not found, creating it
            textCommentsTopic = await CreateTopicAsync
            (
                string.Format(_forumSettings.TextCommentsTopicNameTemplate, textTitle),
                string.Format(_forumSettings.TextCommentsTopicDescriptionTemplate, textTitle),
                _forumSettings.TextsCommentsSectionId,
                textId
            );
        }

        return textCommentsTopic;
    }
}