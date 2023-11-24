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
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests.PrivateMessages;

/// <summary>
/// Service request for importing private messages from old FT
/// </summary>
public class ImportPrivateMessageRequest
{
    /// <summary>
    /// Message content
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }
    
    /// <summary>
    /// Message sender ID
    /// </summary>
    [JsonPropertyName("senderId")]
    public Guid SenderId { get; set; }
    
    /// <summary>
    /// Message receiver ID
    /// </summary>
    [JsonPropertyName("receiverId")]
    public Guid ReceiverId { get; set; }
    
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

    /// <summary>
    /// Is message deleted on sender side?
    /// </summary>
    [JsonPropertyName("isDeletedOnSenderSide")]
    public bool IsDeletedOnSenderSide { get; set; }
    
    /// <summary>
    /// Is message deleted on receiver side?
    /// </summary>
    [JsonPropertyName("isDeletedOnReceiverSide")]
    public bool IsDeletedOnReceiverSide { get; set; }
}