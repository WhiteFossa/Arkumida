using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.PrivateMessages;

/// <summary>
/// Request to send private message
/// </summary>
public class SendPrivateMessageRequest
{
    /// <summary>
    /// Private message itself
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
}