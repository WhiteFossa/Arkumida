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

    public MarkPrivateMessageAsReadResponse
    (
        MarkPrivateMessageAsReadResult result
    )
    {
        Result = result;
    }
}