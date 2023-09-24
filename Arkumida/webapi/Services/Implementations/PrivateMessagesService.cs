using Microsoft.AspNetCore.Identity;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models.Api.DTOs.PrivateMessages;
using webapi.Models.Enums;
using webapi.Models.PrivateMessages;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class PrivateMessagesService : IPrivateMessagesService
{
    private readonly IPrivateMessagesDao _privateMessagesDao;
    private readonly UserManager<CreatureDbo> _userManager;
    private readonly IPrivateMessagesMapper _privateMessagesMapper;
    private readonly IProfilesDao _profilesDao;
    private readonly ICreaturesWithProfilesMapper _creaturesWithProfilesMapper;

    public PrivateMessagesService
    (
        IPrivateMessagesDao privateMessagesDao,
        UserManager<CreatureDbo> userManager,
        IPrivateMessagesMapper privateMessagesMapper,
        IProfilesDao profilesDao,
        ICreaturesWithProfilesMapper creaturesWithProfilesMapper
    )
    {
        _privateMessagesDao = privateMessagesDao;
        _userManager = userManager;
        _privateMessagesMapper = privateMessagesMapper;
        _profilesDao = profilesDao;
        _creaturesWithProfilesMapper = creaturesWithProfilesMapper;
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

    public async Task<Tuple<MarkPrivateMessageAsReadResult, DateTime>> MarkPrivateMessageAsReadAsync(Guid receiverId, Guid messageId)
    {
        var message = await _privateMessagesDao.GetPrivateMessageAsync(messageId);

        if (message.Receiver.Id != receiverId)
        {
            throw new ArgumentException($"Message with ID={messageId} doesn't have receiver with ID={receiverId}!", nameof(messageId));
        }

        if (message.ReadTime.HasValue)
        {
            return new Tuple<MarkPrivateMessageAsReadResult, DateTime>(MarkPrivateMessageAsReadResult.AlreadyMarkedAsRead, message.ReadTime.Value);
        }
        
        message.ReadTime = DateTime.UtcNow;

        await _privateMessagesDao.UpdatePrivateMessageAsync(message);

        return new Tuple<MarkPrivateMessageAsReadResult, DateTime>(MarkPrivateMessageAsReadResult.Successful, message.ReadTime.Value);
    }

    public async Task<IReadOnlyCollection<ConversationSummaryDto>> GetConversationsSummariesAsync(Guid creatureId)
    {
        var confidants = await _privateMessagesDao.GetConfidantsAsync(creatureId);

        var confidantsIds = confidants
            .Select(c => c.Id)
            .ToList();
        
        var profilesDbos = await _profilesDao.MassGetProfilesAsync(confidantsIds);

        var profiles = confidantsIds
            .Select(cid => _creaturesWithProfilesMapper.Map(confidants.Single(c => c.Id == cid), profilesDbos.Single(p => p.Id == cid)))
            .ToDictionary(p => p.Id, p => p);
        
        var lastMessagesTimes = await _privateMessagesDao.GetLastPrivateMessageTimeByConfidantsAsync(creatureId, confidantsIds);

        var unreadMessagesCounts = await _privateMessagesDao.GetUnreadMessagesCountByConfidantsAsync(creatureId, confidantsIds);

        return confidants
            .Select(c => new ConversationSummaryDto(profiles[c.Id].ToDto(), lastMessagesTimes[c.Id], unreadMessagesCounts[c.Id]))
            .ToList();
    }
}