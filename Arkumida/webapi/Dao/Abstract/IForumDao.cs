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

using webapi.Dao.Models.Forum;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with forum sections, topics, messages and so on
/// </summary>
public interface IForumDao
{
    #region Create / update

    /// <summary>
    /// Create forum section
    /// </summary>
    Task<ForumSectionDbo> CreateSectionAsync(ForumSectionDbo sectionDbo);

    /// <summary>
    /// Update forum section
    /// </summary>
    Task UpdateSectionAsync(ForumSectionDbo topicToUpdate);
    
    /// <summary>
    /// Create forum topic and attach it to given section
    /// </summary>
    Task<ForumTopicDbo> CreateTopicAsync(ForumTopicDbo topicDbo, Guid sectionId);

    /// <summary>
    /// Update forum topic
    /// </summary>
    Task UpdateTopicAsync(ForumTopicDbo topicToUpdate);
    
    /// <summary>
    /// Create new forum messageDbo and attach it to given topic
    /// </summary>
    Task<ForumMessageDbo> CreateMessageAsync(ForumMessageDbo messageDbo, Guid topicId);
    
    #endregion
    
    #region Get
    
    /// <summary>
    /// Returns forum section by its ID. If there is no section with such ID will return null
    /// </summary>
    Task<ForumSectionDbo> GetSectionByIdAsync(Guid id);
    
    /// <summary>
    /// Returns section by its name (case-sensitive). If there is no section with given name, will return null
    /// </summary>
    Task<ForumSectionDbo> GetSectionByNameAsync(string name);

    /// <summary>
    /// Returns topic by its ID. If there is no topic with given ID will return null
    /// </summary>
    Task<ForumTopicDbo> GetTopicByIdAsync(Guid id);

    /// <summary>
    /// Get text comments topic by text id. If it doesn't exist - will return null
    /// </summary>
    Task<ForumTopicDbo> GetTextCommentsTopicByTextId(Guid textId);

    /// <summary>
    /// Is topic with given name exist in section?
    /// </summary>
    Task<bool> IsTopicExistInSectionAsync(Guid sectionId, string topicName);

    /// <summary>
    /// Get last messages (by original posting time) from given topic
    /// </summary>
    Task<IReadOnlyCollection<ForumMessageDbo>> GetLastMessagesInTopicAsync(Guid topicId, int skip, int take);
    
    /// <summary>
    /// Like GetTopicByIdAsync(), but messages wouldn't be loaded
    /// </summary>
    Task<ForumTopicDbo> GetTopicWithoutMessagesByIdAsync(Guid id);

    /// <summary>
    /// Get first message in topic
    /// </summary>
    Task<ForumMessageDbo> GetFirstMessageInTopicAsync(Guid topicId);

    /// <summary>
    /// Get last message in topic
    /// </summary>
    Task<ForumMessageDbo> GetLastMessageInTopicAsync(Guid topicId);

    /// <summary>
    /// Get forum message by Id. Will throw an exception if message with given ID doesn't exist
    /// </summary>
    Task<ForumMessageDbo> GetMessageByIdAsync(Guid messageId);
    
    /// <summary>
    /// Get texts comments topics IDs for given texts IDs. Topic ID may be null if there is no comments at all for text
    /// </summary>
    Task<IDictionary<Guid, Guid?>> GetTextsTopicsIdsByTextsIds(IReadOnlyCollection<Guid> textsIds);
    
    /// <summary>
    /// Get messages counts for topics (given topics must exist)
    /// </summary>
    Task<Dictionary<Guid, int>> GetMessagesCountsByTopicsIdsAsync(IReadOnlyCollection<Guid> topicsIds);

    #endregion
}