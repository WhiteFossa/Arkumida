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
    /// Get conversation between sender and receiver
    /// </summary>
    Task<IReadOnlyCollection<PrivateMessageDbo>> GetConversationAsync(Guid receiverId, Guid senderId);
    
    /// <summary>
    /// Get conversation between sender and receiver. Only limit of messages will be returned, after startTime. Messages will be ordered by
    /// sent time
    /// </summary>
    Task<IReadOnlyCollection<PrivateMessageDbo>> GetConversationAfterTimeWithLimitAsync(Guid receiverId, Guid senderId, DateTime startTime, int limit);

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
    Task<IDictionary<Guid, DateTime>> GetLastPrivateMessageTimeByConfidantsAsync(Guid receiverId, IReadOnlyCollection<Guid> sendersIds);

    /// <summary>
    /// Get unread private messages count by senders
    /// </summary>
    Task<IDictionary<Guid, int>> GetUnreadMessagesCountByConfidantsAsync(Guid receiverId, IReadOnlyCollection<Guid> sendersIds);
}