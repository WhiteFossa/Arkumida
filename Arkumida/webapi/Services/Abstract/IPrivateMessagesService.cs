using webapi.Models.Api.DTOs.PrivateMessages;
using webapi.Models.Enums;
using webapi.Models.PrivateMessages;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with private messages
/// </summary>
public interface IPrivateMessagesService
{
    /// <summary>
    /// Get count of unread private messages for given creature
    /// </summary>
    Task<int> GetUnreadPrivateMessagesCountAsync(Guid creatureId);

    /// <summary>
    /// Send a private message to given user
    /// Result:
    /// 0 - is successfull
    /// 1 - new message ID
    /// </summary>
    Task<Tuple<bool, Guid>> SendPrivateMessageAsync(Guid receiverId, Guid senderId, string message);

    /// <summary>
    /// Get a list of private messages, sent to receiver by sender or vice versa, ordered by send time
    /// </summary>
    Task<IReadOnlyCollection<PrivateMessage>> GetConversationAsync(Guid receiverId, Guid senderId);

    /// <summary>
    /// Mark private message as read. Message must belong to receiver
    /// Item1 - mark result, Item2 - mark time (in case of successful marking, otherwise undefiled)
    /// </summary>
    Task<Tuple<MarkPrivateMessageAsReadResult, DateTime>> MarkPrivateMessageAsReadAsync(Guid receiverId, Guid messageId);
    
    /// <summary>
    /// Get conversations summaries for given creature
    /// </summary>
    Task<IReadOnlyCollection<ConversationSummaryDto>> GetConversationsSummariesAsync(Guid creatureId);
}