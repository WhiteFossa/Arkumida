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

using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;

namespace webapi.Dao.Implementations;

public class PrivateMessagesDao : IPrivateMessagesDao
{
    private readonly MainDbContext _dbContext;

    public PrivateMessagesDao
    (
        MainDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> CountUnreadPrivateMessagesAsync(Guid creatureId)
    {
        return await _dbContext
            .PrivateMessages
            .Where(pm => pm.ReadTime == null)
            .Where(pm => pm.Receiver.Id == creatureId)
            .CountAsync();
    }

    public async Task<PrivateMessageDbo> AddPrivateMessageAsync(PrivateMessageDbo privateMessage)
    {
        _ = privateMessage ?? throw new ArgumentNullException(nameof(privateMessage), "Private message must not be null!");

        privateMessage.Receiver = await _dbContext.Users.SingleAsync(c => c.Id == privateMessage.Receiver.Id);
        privateMessage.Sender = await _dbContext.Users.SingleAsync(c => c.Id == privateMessage.Sender.Id);
        
        await _dbContext
            .PrivateMessages
            .AddAsync(privateMessage);
        
        await _dbContext.SaveChangesAsync();

        return privateMessage;
    }

    public async Task<IReadOnlyCollection<PrivateMessageDbo>> GetConversationAfterTimeWithLimitAsync(Guid receiverId, Guid senderId, DateTime afterTime, int limit)
    {
        return await _dbContext
            .PrivateMessages
            .Where(m => (m.Receiver.Id == receiverId) || (m.Receiver.Id == senderId))
            .Where(m => (m.Sender.Id == senderId) || (m.Sender.Id == receiverId))
            .Where(m => m.SentTime > afterTime)
            .Include(m => m.Receiver)
            .Include(m => m.Sender)
            .OrderBy(m => m.SentTime)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<PrivateMessageDbo>> GetConversationBeforeTimeWithLimitAsync(Guid receiverId, Guid senderId, DateTime beforeTime, int limit)
    {
        return await _dbContext
            .PrivateMessages
            .Where(m => (m.Receiver.Id == receiverId) || (m.Receiver.Id == senderId))
            .Where(m => (m.Sender.Id == senderId) || (m.Sender.Id == receiverId))
            .Where(m => m.SentTime < beforeTime)
            .Include(m => m.Receiver)
            .Include(m => m.Sender)
            .OrderByDescending(m => m.SentTime)
            .Take(limit)
            .OrderBy(m => m.SentTime)
            .ToListAsync();
    }

    public async Task<PrivateMessageDbo> GetPrivateMessageAsync(Guid messageId)
    {
        return await _dbContext
            .PrivateMessages
            .Include(m => m.Receiver)
            .Include(m => m.Sender)
            .SingleAsync(m => m.Id == messageId);
    }

    public async Task<PrivateMessageDbo> UpdatePrivateMessageAsync(PrivateMessageDbo privateMessage)
    {
        var privateMessageToUpdate = await _dbContext
            .PrivateMessages
            .SingleAsync(m => m.Id == privateMessage.Id);

        privateMessageToUpdate.Content = privateMessage.Content;
        privateMessageToUpdate.Sender = await _dbContext.Users.SingleAsync(c => c.Id == privateMessage.Sender.Id);
        privateMessageToUpdate.Receiver = await _dbContext.Users.SingleAsync(c => c.Id == privateMessage.Receiver.Id);
        privateMessageToUpdate.SentTime = privateMessage.SentTime;
        privateMessageToUpdate.ReadTime = privateMessage.ReadTime;
        privateMessageToUpdate.IsDeletedOnReceiverSide = privateMessage.IsDeletedOnReceiverSide;
        privateMessageToUpdate.IsDeletedOnSenderSide = privateMessage.IsDeletedOnSenderSide;
        
        await _dbContext.SaveChangesAsync();

        return privateMessageToUpdate;
    }

    public async Task<IReadOnlyCollection<CreatureDbo>> GetConfidantsAsync(Guid creatureId)
    {
        return (await _dbContext
            .PrivateMessages
            .Where(pm => pm.Receiver.Id == creatureId)
            .Include(pm => pm.Sender)
            .Select(pm => pm.Sender)
            .ToListAsync())
            .Concat
            (
                (await _dbContext
                    .PrivateMessages
                    .Where(pm => pm.Sender.Id == creatureId)
                    .Include(pm => pm.Receiver)
                    .Select(pm => pm.Receiver)
                    .ToListAsync())
            )
            .Distinct()
            .ToList();
    }

    public async Task<IDictionary<Guid, DateTime>> GetLastPrivateMessageTimeBySendersAsync(Guid receiverId, IReadOnlyCollection<Guid> sendersIds)
    {
        return await _dbContext
            .PrivateMessages
            .Where(pm => pm.Receiver.Id == receiverId)
            .Where(pm => sendersIds.Contains(pm.Sender.Id))
            .GroupBy(pm => pm.Sender.Id)
            .ToDictionaryAsync(g => g.Key, g => g.Max(pm => pm.SentTime));
    }

    public async Task<IDictionary<Guid, DateTime>> GetLastPrivateMessageTimeByReceiversAsync(Guid senderId, IReadOnlyCollection<Guid> receiversIds)
    {
        return await _dbContext
            .PrivateMessages
            .Where(pm => pm.Sender.Id == senderId)
            .Where(pm => receiversIds.Contains(pm.Receiver.Id))
            .GroupBy(pm => pm.Receiver.Id)
            .ToDictionaryAsync(g => g.Key, g => g.Max(pm => pm.SentTime));
    }

    public async Task<IDictionary<Guid, int>> GetUnreadMessagesCountByConfidantsAsync(Guid receiverId, IReadOnlyCollection<Guid> sendersIds)
    {
        var result = await _dbContext
            .PrivateMessages
            .Where(pm => pm.Receiver.Id == receiverId)
            .Where(pm => sendersIds.Contains(pm.Sender.Id))
            .GroupBy(pm => pm.Sender.Id)
            .ToDictionaryAsync(g => g.Key, g => g.Count(pm => pm.ReadTime == null));

        // In some conversations there are only outgoing messages, adding zeroes for this case
        foreach (var senderId in sendersIds)
        {
            result.TryAdd(senderId, 0);
        }
        
        return result;
    }
}