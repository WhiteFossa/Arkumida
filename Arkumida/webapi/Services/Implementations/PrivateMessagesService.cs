using Microsoft.AspNetCore.Identity;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models.Enums;
using webapi.Models.PrivateMessages;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class PrivateMessagesService : IPrivateMessagesService
{
    private readonly IPrivateMessagesDao _privateMessagesDao;
    private readonly UserManager<CreatureDbo> _userManager;
    private readonly IPrivateMessagesMapper _privateMessagesMapper;

    public PrivateMessagesService
    (
        IPrivateMessagesDao privateMessagesDao,
        UserManager<CreatureDbo> userManager,
        IPrivateMessagesMapper privateMessagesMapper
    )
    {
        _privateMessagesDao = privateMessagesDao;
        _userManager = userManager;
        _privateMessagesMapper = privateMessagesMapper;
    }
    
    public async Task<int> GetUnreadPrivateMessagesCountAsync(Guid creatureId)
    {
        return await _privateMessagesDao.CountUnreadPrivateMessagesAsync(creatureId);
    }

    public async Task<Tuple<bool, Guid>> SendPrivateMessageAsync(Guid receiverId, Guid senderId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return new Tuple<bool, Guid>(false, Guid.NewGuid());
        }

        // Sender
        var senderCreature = await _userManager.FindByIdAsync(senderId.ToString());
        if (senderCreature == null)
        {
            return new Tuple<bool, Guid>(false, Guid.NewGuid());
        }
        
        // Receiver
        var receiverCreature = await _userManager.FindByIdAsync(receiverId.ToString());
        if (receiverCreature == null)
        {
            return new Tuple<bool, Guid>(false, Guid.NewGuid());
        }
        
        var privateMessage = new PrivateMessageDbo()
        {
            Content = message,
            Sender = senderCreature,
            Receiver = receiverCreature,
            SentTime = DateTime.UtcNow,
            ReadTime = null,
            IsDeletedOnReceiverSide = false,
            IsDeletedOnSenderSide = false
        };

        var sentPrivateMessage = await _privateMessagesDao.AddPrivateMessageAsync(privateMessage);

        return new Tuple<bool, Guid>(true, sentPrivateMessage.Id);
    }

    public async Task<IReadOnlyCollection<PrivateMessage>> GetConversationAsync(Guid receiverId, Guid senderId)
    {
        var messages = (await _privateMessagesDao.GetConversationAsync(receiverId, senderId))
            .Where(m => !m.IsDeletedOnReceiverSide)
            .OrderBy(m => m.SentTime);
            
        return _privateMessagesMapper.Map(messages);
    }

    public async Task<MarkPrivateMessageAsReadResult> MarkPrivateMessageAsReadAsync(Guid receiverId, Guid messageId)
    {
        var message = await _privateMessagesDao.GetPrivateMessageAsync(messageId);

        if (message.Receiver.Id != receiverId)
        {
            throw new ArgumentException($"Message with ID={messageId} doesn't have receiver with ID={receiverId}!", nameof(messageId));
        }

        if (message.ReadTime.HasValue)
        {
            return MarkPrivateMessageAsReadResult.AlreadyMarkedAsRead;
        }
        
        message.ReadTime = DateTime.UtcNow;

        await _privateMessagesDao.UpdatePrivateMessageAsync(message);

        return MarkPrivateMessageAsReadResult.Successful;
    }
}