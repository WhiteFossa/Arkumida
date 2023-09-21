using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.DTOs.PrivateMessages;

namespace webapi.Models.Api.Responses.PrivateMessages;

/// <summary>
/// Content of one conversation
/// </summary>
public class ConversationResponse
{
    /// <summary>
    /// Confidant (sender of messages)
    /// </summary>
    [JsonPropertyName("confidant")]
    public CreatureWithProfileDto Confidant { get; }

    /// <summary>
    /// When last message was sent
    /// </summary>
    [JsonPropertyName("lastMessageSentTime")]
    public DateTime LastMessageSentTime { get; }

    /// <summary>
    /// Messages of conversation
    /// </summary>
    [JsonPropertyName("messages")]
    public IReadOnlyCollection<PrivateMessageDto> Messages { get; }

    public ConversationResponse
    (
        CreatureWithProfileDto confidant,
        DateTime lastMessageSentTime,
        IReadOnlyCollection<PrivateMessageDto> messages
    )
    {
        Confidant = confidant ?? throw new ArgumentNullException(nameof(confidant), "Confidant can't be null!");
        LastMessageSentTime = lastMessageSentTime;
        Messages = messages ?? throw new ArgumentNullException(nameof(messages), "Messages can't be null!");
    }
}