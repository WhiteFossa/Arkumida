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

namespace webapi.Models.Api.DTOs.PrivateMessages;

/// <summary>
/// Private message DTO
/// </summary>
public class PrivateMessageDto
{
    /// <summary>
    /// ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Message content
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    /// <summary>
    /// Message sender
    /// </summary>
    [JsonPropertyName("sender")]
    public CreatureDto Sender { get; set; }
    
    /// <summary>
    /// Message receiver
    /// </summary>
    [JsonPropertyName("receiver")]
    public CreatureDto Receiver { get; set; }

    /// <summary>
    /// Message was sent at this time
    /// </summary>
    [JsonPropertyName("sentTime")]
    public DateTime SentTime { get; set; }
    
    /// <summary>
    /// Message was read (if was) at this time
    /// </summary>
    [JsonPropertyName("readTime")]
    public DateTime? ReadTime { get; set; }

    public PrivateMessageDto
    (
        Guid id,
        string content,
        CreatureDto sender,
        CreatureDto receiver,
        DateTime sentTime,
        DateTime? readTime
    )
    {
        Id = id;

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Private message content can't be empty!", nameof(content));
        }

        Content = content;

        Sender = sender ?? throw new ArgumentNullException(nameof(sender), "Sender can't be null!");
        Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver), "Receiver can't be null!");

        SentTime = sentTime;
        ReadTime = readTime;
    }
}