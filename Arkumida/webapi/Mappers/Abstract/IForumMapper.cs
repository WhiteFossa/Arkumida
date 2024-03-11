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

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for everything forum-related
/// Experimental service-style mapper
/// </summary>
public interface IForumMapper
{
    #region Messages
    
    Task<IReadOnlyCollection<ForumMessage>> MapAsync(IEnumerable<ForumMessageDbo> messages);

    Task<ForumMessage> MapAsync(ForumMessageDbo message);

    ForumMessageDbo Map(ForumMessage message);

    IReadOnlyCollection<ForumMessageDbo> Map(IEnumerable<ForumMessage> messages);
    
    #endregion
    
    #region Topics

    Task<IReadOnlyCollection<ForumTopic>> MapAsync(IEnumerable<ForumTopicDbo> topics);

    Task<ForumTopic> MapAsync(ForumTopicDbo topic);

    ForumTopicDbo Map(ForumTopic topic);

    IReadOnlyCollection<ForumTopicDbo> Map(IEnumerable<ForumTopic> topics);

    #endregion
    
    #region Sections

    Task<IReadOnlyCollection<ForumSection>> MapAsync(IEnumerable<ForumSectionDbo> sections);

    Task<ForumSection> MapAsync(ForumSectionDbo section);

    ForumSectionDbo Map(ForumSection section);

    IReadOnlyCollection<ForumSectionDbo> Map(IEnumerable<ForumSection> sections);

    #endregion
}