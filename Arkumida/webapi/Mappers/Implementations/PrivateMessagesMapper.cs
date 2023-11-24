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
using webapi.Mappers.Abstract;
using webapi.Models.PrivateMessages;

namespace webapi.Mappers.Implementations;

public class PrivateMessagesMapper : IPrivateMessagesMapper
{
    private readonly ICreaturesMapper _creaturesMapper;

    public PrivateMessagesMapper(ICreaturesMapper creaturesMapper)
    {
        _creaturesMapper = creaturesMapper;
    }
    
    public IReadOnlyCollection<PrivateMessage> Map(IEnumerable<PrivateMessageDbo> privateMessages)
    {
        if (privateMessages == null)
        {
            return null;
        }

        return privateMessages.Select(pm => Map(pm)).ToList();
    }

    public PrivateMessage Map(PrivateMessageDbo privateMessage)
    {
        if (privateMessage == null)
        {
            return null;
        }

        return new PrivateMessage()
        {
            Id = privateMessage.Id,
            Content = privateMessage.Content,
            Sender = _creaturesMapper.Map(privateMessage.Sender),
            Receiver = _creaturesMapper.Map(privateMessage.Receiver),
            SentTime = privateMessage.SentTime,
            ReadTime = privateMessage.ReadTime,
            IsDeletedOnReceiverSide = privateMessage.IsDeletedOnReceiverSide,
            IsDeletedOnSenderSide = privateMessage.IsDeletedOnSenderSide
        };
    }

    public PrivateMessageDbo Map(PrivateMessage privateMessage)
    {
        if (privateMessage == null)
        {
            return null;
        }

        return new PrivateMessageDbo()
        {
            Id = privateMessage.Id,
            Content = privateMessage.Content,
            Sender = _creaturesMapper.Map(privateMessage.Sender),
            Receiver = _creaturesMapper.Map(privateMessage.Receiver),
            SentTime = privateMessage.SentTime,
            ReadTime = privateMessage.ReadTime,
            IsDeletedOnReceiverSide = privateMessage.IsDeletedOnReceiverSide,
            IsDeletedOnSenderSide = privateMessage.IsDeletedOnSenderSide
        };
    }

    public IReadOnlyCollection<PrivateMessageDbo> Map(IEnumerable<PrivateMessage> privateMessages)
    {
        if (privateMessages == null)
        {
            return null;
        }

        return privateMessages.Select(pm => Map(pm)).ToList();
    }
}