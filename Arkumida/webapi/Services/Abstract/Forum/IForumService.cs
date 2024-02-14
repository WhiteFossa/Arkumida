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
using webapi.Models.Forum;
using webapi.Models.Forum.Infos;

namespace webapi.Services.Abstract.Forum;

/// <summary>
/// Service to work with forum
/// </summary>
public interface IForumService
{
    #region Sections

    /// <summary>
    /// Create empty forum section. If ID is provided, then section will have given ID (however, ID check will be performed in this case)
    /// </summary>
    Task<ForumSection> CreateSectionAsync
    (
        string name,
        string description,
        Guid authorId,
        Guid? id = null
    );

    /// <summary>
    /// Get forum section by its ID
    /// </summary>
    Task<ForumSection> GetSectionByIdAsync(Guid id);
    
    /// <summary>
    /// Get forum section by its name (case-sensitive).
    /// </summary>
    Task<ForumSection> GetSectionByNameAsync(string name);

    #endregion

    #region Topics
    
    /// <summary>
    /// Get topic by its ID
    /// </summary>
    Task<ForumTopic> GetTopicByIdAsync(Guid id);

    /// <summary>
    /// Get special (text comments) topic by text ID
    /// </summary>
    Task<ForumTopic> GetTextCommentsTopicByTextIdAsync(Guid textId);

    /// <summary>
    /// Create empty forum topic.
    /// If textId have value, then this topic will be related to specified text (only one topic can be specified to text)
    /// </summary>
    Task<ForumTopic> CreateTopicAsync
    (
        string name,
        string description,
        Guid sectionId,
        Guid? textId = null
    );

    /// <summary>
    /// Get topic metadata. Will return null if topic with given ID doesn't exist
    /// </summary>
    Task<ForumTopicInfo> GetTopicInfoAsync(Guid topicId);
    
    #endregion
    
    #region Messages

    /// <summary>
    /// Get last messages in topic, ordered by original post date
    /// </summary>
    Task<IReadOnlyCollection<ForumMessage>> GetLastMessagesInTopicAsync(Guid topicId, int skip, int take);

    /// <summary>
    /// Get messages count in topic
    /// </summary>
    Task<int> GetTopicMessagesCountAsync(Guid topicId);
    
    #endregion
    
    #region Texts comments (special messages)

    /// <summary>
    /// Add comment to text. Creates special topic if there is no topic for text comments
    /// </summary>
    Task<ForumMessage> AddTextCommentAsync(Guid textId, ForumMessage messageToAdd);

    /// <summary>
    /// Get last (by original posting date) comments for given text
    /// </summary>
    Task<IReadOnlyCollection<ForumMessage>> GetLastCommentsByTextAsync(Guid textId, int skip, int take);

    /// <summary>
    /// Get total comments count for given text
    /// </summary>
    Task<int> GetTextCommentsCountAsync(Guid textId);

    #endregion
}