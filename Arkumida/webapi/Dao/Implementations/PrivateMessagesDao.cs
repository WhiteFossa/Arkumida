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

    public async Task<IReadOnlyCollection<PrivateMessageDbo>> GetConversationAsync(Guid receiverId, Guid senderId)
    {
        return await _dbContext
            .PrivateMessages
            .Where(m => m.Receiver.Id == receiverId)
            .Where(m => m.Sender.Id == senderId)
            .Include(m => m.Receiver)
            .Include(m => m.Sender)
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
        return await _dbContext
            .PrivateMessages
            .Where(pm => pm.Receiver.Id == creatureId)
            .Include(pm => pm.Sender)
            .Select(pm => pm.Sender)
            .ToListAsync();
    }

    public async Task<IDictionary<Guid, DateTime>> GetLastPrivateMessageTimeByConfidantsAsync(Guid receiverId, IReadOnlyCollection<Guid> sendersIds)
    {
        return await _dbContext
            .PrivateMessages
            .Where(pm => pm.Receiver.Id == receiverId)
            .Where(pm => sendersIds.Contains(pm.Sender.Id))
            .GroupBy(pm => pm.Sender.Id)
            .ToDictionaryAsync(g => g.Key, g => g.Max(pm => pm.SentTime));
    }

    public async Task<IDictionary<Guid, int>> GetUnreadMessagesCountByConfidantsAsync(Guid receiverId, IReadOnlyCollection<Guid> sendersIds)
    {
        return await _dbContext
            .PrivateMessages
            .Where(pm => pm.Receiver.Id == receiverId)
            .Where(pm => sendersIds.Contains(pm.Sender.Id))
            .GroupBy(pm => pm.Sender.Id)
            .ToDictionaryAsync(g => g.Key, g => g.Count(pm => pm.ReadTime == null));
    }
}