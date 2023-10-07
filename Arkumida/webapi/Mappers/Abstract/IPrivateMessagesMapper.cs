using webapi.Dao.Models;
using webapi.Models;
using webapi.Models.PrivateMessages;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for private messages
/// </summary>
public interface IPrivateMessagesMapper
{
    IReadOnlyCollection<PrivateMessage> Map(IEnumerable<PrivateMessageDbo> privateMessages);

    PrivateMessage Map(PrivateMessageDbo privateMessage);

    PrivateMessageDbo Map(PrivateMessage privateMessage);

    IReadOnlyCollection<PrivateMessageDbo> Map(IEnumerable<PrivateMessage> privateMessages);
}