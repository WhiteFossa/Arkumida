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

using webapi.Models.Api.DTOs.Forum;
using webapi.Models.Creatures;
using webapi.Services.Abstract;

namespace webapi.Models.Forum;

/// <summary>
/// Forum message
/// </summary>
public class ForumMessage
{
    /// <summary>
    /// Message ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Message author
    /// </summary>
    public CreatureWithProfile Author { get; set; }

    /// <summary>
    /// This message is reply to given message. May be null
    /// </summary>
    public ForumMessage ReplyTo { get; set; }

    /// <summary>
    /// When the message was initially posted
    /// </summary>
    public DateTime PostTime { get; set; }

    /// <summary>
    /// When the message was updated last time (initially equal to PostTime)
    /// </summary>
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// The message itself
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// The message belongs to this topic
    /// </summary>
    public Guid TopicId { get; set; }

    /// <summary>
    /// Convert to forum message DTO
    /// </summary>
    public ForumMessageDto ToDto(ITextUtilsService textUtilsService)
    {
        return new ForumMessageDto()
        {
            Id = Id,
            Author = Author.ToDto(),
            ReplyTo = ReplyTo?.ToDto(textUtilsService),
            PostTime = PostTime,
            LastUpdateTime = LastUpdateTime,
            PlaintextMessage = Message,
            ParsedMessage = textUtilsService.ParseTextToElements(Message, new List<TextFile>()), // For now we act like forum messages have no attached files
            TopicId = TopicId
        };
    }
}