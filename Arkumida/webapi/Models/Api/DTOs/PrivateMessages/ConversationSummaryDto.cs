using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs.PrivateMessages;

public class ConversationSummaryDto
{
    /// <summary>
    /// Confidant (sender of messages)
    /// </summary>
    [JsonPropertyName("confidant")]
    public CreatureWithProfileDto Confidant { get; set; }
    
    /// <summary>
    /// When last message was sent
    /// </summary>
    [JsonPropertyName("lastMessageSentTime")]
    public DateTime LastMessageSentTime { get; set; }

    /// <summary>
    /// Amount of unread messages in this conversation
    /// </summary>
    [JsonPropertyName("unreadMessagesCount")]
    public int UnreadMessagesCount { get; set; }

    public ConversationSummaryDto
    (
        CreatureWithProfileDto confidant,
        DateTime lastMessageSentTime,
        int unreadMessagesCount
    )
    {
        Confidant = confidant ?? throw new ArgumentNullException(nameof(confidant), "Confidant can't be null!");
        LastMessageSentTime = lastMessageSentTime;

        if (unreadMessagesCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unreadMessagesCount), "Negative unread messages count!");
        }
        UnreadMessagesCount = unreadMessagesCount;
    }
}