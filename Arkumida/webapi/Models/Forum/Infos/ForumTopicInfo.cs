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

using webapi.Models.Api.DTOs.Forum.Infos;
using webapi.Models.Creatures;
using webapi.Services.Abstract;

namespace webapi.Models.Forum.Infos;

/// <summary>
/// Metadata about forum topic
/// </summary>
public class ForumTopicInfo
{
    /// <summary>
    /// Topic ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Topic name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Topic description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Messages count
    /// </summary>
    public int MessagesCount { get; set; }

    /// <summary>
    /// First message in topic
    /// </summary>
    public ForumMessage FirstMessage { get; set; }

    /// <summary>
    /// Last message in topic
    /// </summary>
    public ForumMessage LastMessage { get; set; }

    /// <summary>
    /// If this field is not null, then this topic is comments topic for given text
    /// </summary>
    public Text CommentsForText { get; set; }

    /// <summary>
    /// To DTO
    /// </summary>
    public ForumTopicInfoDto ToDto(ITextUtilsService textUtilsService)
    {
        return new ForumTopicInfoDto
        (
            Id,
            Name,
            Description,
            MessagesCount,
            FirstMessage.ToDto(),
            LastMessage.ToDto(),
            CommentsForText.ToDto(textUtilsService)
        );
    }
}