#region License
// Furtails Importer - Importer from furtails.pw database to Arkumida
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

namespace furtails_importer.WebClientStuff.Dtos;

public class AddTextCommentDto
{
    /// <summary>
    /// Message author ID
    /// </summary>
    [JsonPropertyName("authorId")]
    public Guid AuthorId { get; set; }

    /// <summary>
    /// This message is reply to given message. May be null
    /// </summary>
    [JsonPropertyName("replyTo")]
    public Guid? ReplyTo { get; set; }

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
    /// The message itself
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
}