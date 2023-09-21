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
    /// Get private message by ID
    /// </summary>
    Task<PrivateMessageDbo> GetPrivateMessageAsync(Guid messageId);

    /// <summary>
    /// Update private message
    /// </summary>
    Task<PrivateMessageDbo> UpdatePrivateMessageAsync(PrivateMessageDbo privateMessage);
}