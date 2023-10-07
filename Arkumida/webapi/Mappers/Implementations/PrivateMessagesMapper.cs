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