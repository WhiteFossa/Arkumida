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

using webapi.Models.Api.DTOs.PrivateMessages;
using webapi.Models.Api.Requests.PrivateMessages;
using webapi.Models.Enums;
using webapi.Models.PrivateMessages;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with private messages
/// </summary>
public interface IPrivateMessagesService
{
    /// <summary>
    /// Get count of unread private messages for given creature
    /// </summary>
    Task<int> GetUnreadPrivateMessagesCountAsync(Guid creatureId);

    /// <summary>
    /// Send a private message to given user
    /// Result:
    /// 1 - is successfull
    /// 2 - new message
    /// </summary>
    Task<Tuple<bool, PrivateMessageDto>> SendPrivateMessageAsync(Guid receiverId, Guid senderId, string message);

    /// <summary>
    /// THIS METHOD IS INTENDED FOR FURTAILS-IMPORTER USE ONLY!
    /// Import private message (kinda dirty that we are using request instead of request -> DTO, but we are lazy and believe that this is OK for import method)
    /// </summary>
    Task<bool> ImportPrivateMessageAsync(ImportPrivateMessageRequest importRequest);
    
    /// <summary>
    /// Get a list of private messages, sent to receiver by sender or vice versa, ordered by send time
    /// Only messages sent later than afterTime will be returned, and no more than limit of messages
    /// </summary>
    Task<IReadOnlyCollection<PrivateMessage>> GetConversationAfterTimeWithLimitAsync(Guid receiverId, Guid senderId, DateTime afterTime, int limit);
    
    /// <summary>
    /// Get a list of private messages, sent to receiver by sender or vice versa, ordered by send time
    /// Only messages sent earlier than beforeTime will be returned, and no more than limit of messages
    /// </summary>
    Task<IReadOnlyCollection<PrivateMessage>> GetConversationBeforeTimeWithLimitAsync(Guid receiverId, Guid senderId, DateTime beforeTime, int limit);

    /// <summary>
    /// Mark private message as read. Message must belong to receiver
    /// Item1 - mark result, Item2 - mark time (in case of successful marking, otherwise undefiled)
    /// </summary>
    Task<Tuple<MarkPrivateMessageAsReadResult, DateTime>> MarkPrivateMessageAsReadAsync(Guid receiverId, Guid messageId);
    
    /// <summary>
    /// Get conversations summaries for given creature
    /// </summary>
    Task<IReadOnlyCollection<ConversationSummaryDto>> GetConversationsSummariesAsync(Guid creatureId);
}