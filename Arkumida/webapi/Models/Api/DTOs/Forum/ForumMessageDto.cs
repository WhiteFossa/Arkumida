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

using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs.Creatures;
using webapi.Models.Api.DTOs.TextsComments;
using webapi.Models.Forum;

namespace webapi.Models.Api.DTOs.Forum;

/// <summary>
/// Forum message DTO
/// </summary>
public class ForumMessageDto
{
    /// <summary>
    /// Message ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Message author
    /// </summary>
    [JsonPropertyName("author")]
    public CreatureWithProfileDto Author { get; set; }

    /// <summary>
    /// This message is a reply to the given message. May be null
    /// </summary>
    [JsonPropertyName("replyTo")]
    public ForumMessageDto ReplyTo { get; set; }

    /// <summary>
    /// When the message was initially posted
    /// </summary>
    [JsonPropertyName("postTime")]
    public DateTime PostTime { get; set; }

    /// <summary>
    /// When the message was updated last time (initially equal to PostTime)
    /// </summary>
    [JsonPropertyName("lastUpdateTime")]
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// The message itself (plaintext)
    /// </summary>
    [JsonPropertyName("messagePlaintext")]
    public string PlaintextMessage { get; set; }
    
    /// <summary>
    /// Parsed messaged (as text elements set)
    /// </summary>
    [JsonPropertyName("messageParsed")]
    public IReadOnlyCollection<TextElementDto> ParsedMessage { get; set; }

    public ForumMessage ToForumMessage()
    {
        return new ForumMessage()
        {
            Id = Id,
            Author = Author.ToCreatureWithProfile(),
            ReplyTo = ReplyTo?.ToForumMessage(),
            PostTime = PostTime,
            LastUpdateTime = LastUpdateTime,
            Message = PlaintextMessage
        };
    }
}