using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs.PrivateMessages;

namespace webapi.Models.Api.Responses.PrivateMessages;

/// <summary>
/// Response on private message sending
/// </summary>
public class SentPrivateMessageResponse
{
    /// <summary>
    /// Is message sent successfully
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; }

    /// <summary>
    /// Sent message
    /// </summary>
    [JsonPropertyName("message")]
    public PrivateMessageDto Message { get; }

    public SentPrivateMessageResponse
    (
        bool isSuccessful,
        PrivateMessageDto message
    )
    {
        IsSuccessful = isSuccessful;
        Message = message ?? throw new ArgumentNullException(nameof(message), "Sent message must not be null!");
    }
}