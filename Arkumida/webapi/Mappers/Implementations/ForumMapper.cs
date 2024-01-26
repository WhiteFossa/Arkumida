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

using webapi.Dao.Abstract;
using webapi.Dao.Models.Forum;
using webapi.Mappers.Abstract;
using webapi.Models.Forum;

namespace webapi.Mappers.Implementations;

public class ForumMapper : IForumMapper
{
    private readonly ICreaturesWithProfilesMapper _creaturesWithProfilesMapper;
    private readonly IProfilesDao _profilesDao;
    private readonly ITextsMapper _textsMapper;

    public ForumMapper
    (
        ICreaturesWithProfilesMapper creaturesWithProfilesMapper,
        IProfilesDao profilesDao,
        ITextsMapper textsMapper
    )
    {
        _creaturesWithProfilesMapper = creaturesWithProfilesMapper;
        _profilesDao = profilesDao;
        _textsMapper = textsMapper;
    }
    
    public async Task<IReadOnlyCollection<ForumMessage>> MapAsync(IEnumerable<ForumMessageDbo> messages)
    {
        if (messages == null)
        {
            return null;
        }

        var result = new List<ForumMessage>();
        
        foreach (var message in messages)
        {
            result.Add(await MapAsync(message));
        }

        return result;
    }

    public async Task<ForumMessage> MapAsync(ForumMessageDbo message)
    {
        if (message == null)
        {
            return null;
        }

        var authorProfile = await _profilesDao.GetProfileAsync(message.Author.Id);
        
        return new ForumMessage()
        {
            Id = message.Id,
            Author = _creaturesWithProfilesMapper.Map(message.Author, authorProfile),
            ReplyTo = await MapAsync(message.ReplyTo),
            PostTime = message.PostTime,
            LastUpdateTime = message.LastUpdateTime,
            Message = message.Message
        };
    }

    public ForumMessageDbo Map(ForumMessage message)
    {
        if (message == null)
        {
            return null;
        }

        return new ForumMessageDbo()
        {
            Id = message.Id,
            Author = _creaturesWithProfilesMapper.Map(message.Author).Item1,
            ReplyTo = Map(message.ReplyTo),
            PostTime = message.PostTime,
            LastUpdateTime = message.LastUpdateTime,
            Message = message.Message
        };
    }

    public IReadOnlyCollection<ForumMessageDbo> Map(IEnumerable<ForumMessage> messages)
    {
        if (messages == null)
        {
            return null;
        }

        return messages.Select(m => Map(m)).ToList();
    }

    public async Task<IReadOnlyCollection<ForumTopic>> MapAsync(IEnumerable<ForumTopicDbo> topics)
    {
        if (topics == null)
        {
            return null;
        }

        var result = new List<ForumTopic>();

        foreach (var topic in topics)
        {
            result.Add(await MapAsync(topic));
        }

        return result;
    }

    public async Task<ForumTopic> MapAsync(ForumTopicDbo topic)
    {
        if (topic == null)
        {
            return null;
        }

        return new ForumTopic()
        {
            Id = topic.Id,
            Name = topic.Name,
            Description = topic.Description,
            Messages = topic.Messages != null ? (await MapAsync(topic.Messages)).ToList() : null,
            CommentsForText = _textsMapper.Map(topic.CommentsForText) // TODO: Think about mapping authors etc
        };
    }

    public ForumTopicDbo Map(ForumTopic topic)
    {
        if (topic == null)
        {
            return null;
        }

        return new ForumTopicDbo()
        {
            Id = topic.Id,
            Name = topic.Name,
            Description = topic.Description,
            Messages = topic.Messages!= null ? Map(topic.Messages).ToList() : null,
            CommentsForText = _textsMapper.Map(topic.CommentsForText)
        };
    }

    public IReadOnlyCollection<ForumTopicDbo> Map(IEnumerable<ForumTopic> topics)
    {
        if (topics == null)
        {
            return null;
        }

        return topics.Select(t => Map(t)).ToList();
    }

    public async Task<IReadOnlyCollection<ForumSection>> MapAsync(IEnumerable<ForumSectionDbo> sections)
    {
        if (sections == null)
        {
            return null;
        }

        var result = new List<ForumSection>();
        foreach (var section in sections)
        {
            result.Add(await MapAsync(section));
        }

        return result;
    }

    public async Task<ForumSection> MapAsync(ForumSectionDbo section)
    {
        if (section == null)
        {
            return null;
        }

        var authorProfile = await _profilesDao.GetProfileAsync(section.Author.Id);
        
        return new ForumSection()
        {
            Id = section.Id,
            Name = section.Name,
            Description = section.Description,
            CreationTime = section.CreationTime,
            Author = _creaturesWithProfilesMapper.Map(section.Author, authorProfile),
            Subsections = (await MapAsync(section.Subsections)).ToList(),
            Topics = (await MapAsync(section.Topics)).ToList()
        };
    }

    public ForumSectionDbo Map(ForumSection section)
    {
        if (section == null)
        {
            return null;
        }

        return new ForumSectionDbo()
        {
            Id = section.Id,
            Name = section.Name,
            Description = section.Description,
            CreationTime = section.CreationTime,
            Author = _creaturesWithProfilesMapper.Map(section.Author).Item1,
            Subsections = Map(section.Subsections).ToList(),
            Topics = Map(section.Topics).ToList()
        };
    }

    public IReadOnlyCollection<ForumSectionDbo> Map(IEnumerable<ForumSection> sections)
    {
        if (sections == null)
        {
            return null;
        }

        return sections.Select(s => Map(s)).ToList();
    }
}