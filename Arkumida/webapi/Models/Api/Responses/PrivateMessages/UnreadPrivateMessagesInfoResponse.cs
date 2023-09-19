using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.PrivateMessages;

/// <summary>
/// Information about new private messages
/// </summary>
public class UnreadPrivateMessagesInfoResponse
{
    /// <summary>
    /// Count of unread private messages
    /// </summary>
    [JsonPropertyName("unreadMessagesCount")]
    public int UnreadMessagesCount { get; }

    public UnreadPrivateMessagesInfoResponse
    (
        int unreadMessagesCount
    )
    {
        if (unreadMessagesCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unreadMessagesCount), unreadMessagesCount, "Unread messages count must not be negative!");
        }
        UnreadMessagesCount = unreadMessagesCount;
    }
}