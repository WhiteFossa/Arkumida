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

using Microsoft.AspNetCore.Identity;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models.Api.DTOs.PrivateMessages;
using webapi.Models.Api.Requests.PrivateMessages;
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

    public async Task<Tuple<bool, PrivateMessageDto>> SendPrivateMessageAsync(Guid receiverId, Guid senderId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return new Tuple<bool, PrivateMessageDto>(false, null);
        }

        // Sender
        var senderCreature = await _userManager.FindByIdAsync(senderId.ToString());
        if (senderCreature == null)
        {
            return new Tuple<bool, PrivateMessageDto>(false, null);
        }
        
        // Receiver
        var receiverCreature = await _userManager.FindByIdAsync(receiverId.ToString());
        if (receiverCreature == null)
        {
            return new Tuple<bool, PrivateMessageDto>(false, null);
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

        var sentPrivateMessageDbo = await _privateMessagesDao.AddPrivateMessageAsync(privateMessage);
        
        var sentPrivateMessage = _privateMessagesMapper.Map(sentPrivateMessageDbo);

        return new Tuple<bool, PrivateMessageDto>(true, sentPrivateMessage.ToDto());
    }

    public async Task<bool> ImportPrivateMessageAsync(ImportPrivateMessageRequest importRequest)
    {
        // Sender
        var senderCreature = await _userManager.FindByIdAsync(importRequest.SenderId.ToString());
        if (senderCreature == null)
        {
            return false;
        }
        
        // Receiver
        var receiverCreature = await _userManager.FindByIdAsync(importRequest.ReceiverId.ToString());
        if (receiverCreature == null)
        {
            return false;
        }
        
        var privateMessage = new PrivateMessageDbo()
        {
            Content = importRequest.Content,
            Sender = senderCreature,
            Receiver = receiverCreature,
            SentTime = importRequest.SentTime,
            ReadTime = importRequest.ReadTime,
            IsDeletedOnReceiverSide = importRequest.IsDeletedOnReceiverSide,
            IsDeletedOnSenderSide = importRequest.IsDeletedOnSenderSide
        };

        await _privateMessagesDao.AddPrivateMessageAsync(privateMessage);

        return true;
    }

    public async Task<IReadOnlyCollection<PrivateMessage>> GetConversationAfterTimeWithLimitAsync(Guid receiverId, Guid senderId, DateTime afterTime, int limit)
    {
        return _privateMessagesMapper.Map(await _privateMessagesDao.GetConversationAfterTimeWithLimitAsync(receiverId, senderId, afterTime, limit));
    }

    public async Task<IReadOnlyCollection<PrivateMessage>> GetConversationBeforeTimeWithLimitAsync(Guid receiverId, Guid senderId, DateTime beforeTime, int limit)
    {
        return _privateMessagesMapper.Map(await _privateMessagesDao.GetConversationBeforeTimeWithLimitAsync(receiverId, senderId, beforeTime, limit));
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
        
        var lastMessagesTimesBySenders = await _privateMessagesDao.GetLastPrivateMessageTimeBySendersAsync(creatureId, confidantsIds);
        var lastMessagesTimesByReceivers = await _privateMessagesDao.GetLastPrivateMessageTimeByReceiversAsync(creatureId, confidantsIds);

        var unreadMessagesCounts = await _privateMessagesDao.GetUnreadMessagesCountByConfidantsAsync(creatureId, confidantsIds);

        return confidants
            .Select(c =>
            {
                var lastMessageTimestampBySender = (lastMessagesTimesBySenders.ContainsKey(c.Id) ? lastMessagesTimesBySenders[c.Id] : DateTime.MinValue).Ticks;
                var lastMessageTimestampByReceiver = (lastMessagesTimesByReceivers.ContainsKey(c.Id) ? lastMessagesTimesByReceivers[c.Id] : DateTime.MinValue).Ticks;
                
                return new ConversationSummaryDto
                (
                    profiles[c.Id].ToDto(),
                    new DateTime(Math.Max(lastMessageTimestampBySender, lastMessageTimestampByReceiver), DateTimeKind.Utc),
                    unreadMessagesCounts[c.Id]
                );
            })
            .ToList();
    }
}