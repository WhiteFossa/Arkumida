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

public class ConversationSummaryDto
{
    /// <summary>
    /// Confidant (sender of messages)
    /// </summary>
    [JsonPropertyName("confidant")]
    public CreatureWithProfileDto Confidant { get; set; }
    
    /// <summary>
    /// When last message was sent
    /// </summary>
    [JsonPropertyName("lastMessageSentTime")]
    public DateTime LastMessageSentTime { get; set; }

    /// <summary>
    /// Amount of unread messages in this conversation
    /// </summary>
    [JsonPropertyName("unreadMessagesCount")]
    public int UnreadMessagesCount { get; set; }

    public ConversationSummaryDto
    (
        CreatureWithProfileDto confidant,
        DateTime lastMessageSentTime,
        int unreadMessagesCount
    )
    {
        Confidant = confidant ?? throw new ArgumentNullException(nameof(confidant), "Confidant can't be null!");
        LastMessageSentTime = lastMessageSentTime;

        if (unreadMessagesCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unreadMessagesCount), "Negative unread messages count!");
        }
        UnreadMessagesCount = unreadMessagesCount;
    }
}