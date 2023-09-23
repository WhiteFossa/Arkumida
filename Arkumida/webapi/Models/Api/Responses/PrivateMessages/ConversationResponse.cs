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
    /// Messages of conversation
    /// </summary>
    [JsonPropertyName("messages")]
    public IReadOnlyCollection<PrivateMessageDto> Messages { get; }

    public ConversationResponse
    (
        IReadOnlyCollection<PrivateMessageDto> messages
    )
    {
        Messages = messages ?? throw new ArgumentNullException(nameof(messages), "Messages can't be null!");
    }
}