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

using webapi.Dao.Models;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with private messages
/// </summary>
public interface IPrivateMessagesDao
{
    /// <summary>
    /// Count unread messages for creature
    /// </summary>
    Task<int> CountUnreadPrivateMessagesAsync(Guid creatureId);

    /// <summary>
    /// Add private message (all fields except message ID must be populated)
    /// </summary>
    Task<PrivateMessageDbo> AddPrivateMessageAsync(PrivateMessageDbo privateMessage);
    
    /// <summary>
    /// Get conversation between sender and receiver. Only limit of messages will be returned, sent after afterTime. Messages will be ordered by
    /// sent time (earliest first)
    /// </summary>
    Task<IReadOnlyCollection<PrivateMessageDbo>> GetConversationAfterTimeWithLimitAsync(Guid receiverId, Guid senderId, DateTime afterTime, int limit);
    
    /// <summary>
    /// Get conversation between sender and receiver. Only limit of messages will be returned, sent before beforeTime. Messages will be ordered by
    /// sent time (earliest first)
    /// </summary>
    Task<IReadOnlyCollection<PrivateMessageDbo>> GetConversationBeforeTimeWithLimitAsync(Guid receiverId, Guid senderId, DateTime beforeTime, int limit);

    /// <summary>
    /// Get private message by ID
    /// </summary>
    Task<PrivateMessageDbo> GetPrivateMessageAsync(Guid messageId);

    /// <summary>
    /// Update private message
    /// </summary>
    Task<PrivateMessageDbo> UpdatePrivateMessageAsync(PrivateMessageDbo privateMessage);

    /// <summary>
    /// Get all creatures, who talked with given creature at least once
    /// </summary>
    Task<IReadOnlyCollection<CreatureDbo>> GetConfidantsAsync(Guid creatureId);

    /// <summary>
    /// Get last private messages times from given senders
    /// </summary>
    Task<IDictionary<Guid, DateTime>> GetLastPrivateMessageTimeBySendersAsync(Guid receiverId, IReadOnlyCollection<Guid> sendersIds);
    
    /// <summary>
    /// Get last private messages times sent to given receivers
    /// </summary>
    Task<IDictionary<Guid, DateTime>> GetLastPrivateMessageTimeByReceiversAsync(Guid senderId, IReadOnlyCollection<Guid> receiversIds);

    /// <summary>
    /// Get unread private messages count by senders
    /// </summary>
    Task<IDictionary<Guid, int>> GetUnreadMessagesCountByConfidantsAsync(Guid receiverId, IReadOnlyCollection<Guid> sendersIds);
}