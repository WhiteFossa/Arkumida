using System.Text.Json.Serialization;

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
    /// Sent message ID
    /// </summary>
    [JsonPropertyName("sentMessageId")]
    public Guid SentMessageId { get; }

    public SentPrivateMessageResponse
    (
        bool isSuccessful,
        Guid sentMessageId
    )
    {
        IsSuccessful = isSuccessful;
        SentMessageId = sentMessageId;
    }
}