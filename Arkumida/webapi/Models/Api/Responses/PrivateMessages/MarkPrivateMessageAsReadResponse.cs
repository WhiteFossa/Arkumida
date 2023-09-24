using System.Text.Json.Serialization;
using webapi.Models.Enums;

namespace webapi.Models.Api.Responses.PrivateMessages;

/// <summary>
/// Mark message as read result
/// </summary>
public class MarkPrivateMessageAsReadResponse
{
    /// <summary>
    /// Marking result
    /// </summary>
    [JsonPropertyName("result")]
    public MarkPrivateMessageAsReadResult Result { get; }

    /// <summary>
    /// If message was successfully marked as read, here will be the time of mark
    /// </summary>
    [JsonPropertyName("markTime")]
    public DateTime MarkTime { get; }

    public MarkPrivateMessageAsReadResponse
    (
        MarkPrivateMessageAsReadResult result,
        DateTime markTime
    )
    {
        Result = result;
        MarkTime = markTime;
    }
}