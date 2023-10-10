using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests.PrivateMessages;

/// <summary>
/// Service request for importing private messages from old FT
/// </summary>
public class ImportPrivateMessageRequest
{
    /// <summary>
    /// Message content
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }
    
    /// <summary>
    /// Message sender ID
    /// </summary>
    [JsonPropertyName("senderId")]
    public Guid SenderId { get; set; }
    
    /// <summary>
    /// Message receiver ID
    /// </summary>
    [JsonPropertyName("receiverId")]
    public Guid ReceiverId { get; set; }
    
    /// <summary>
    /// Message was sent at this time
    /// </summary>
    [JsonPropertyName("sentTime")]
    public DateTime SentTime { get; set; }
    
    /// <summary>
    /// Message was read (if was) at this time
    /// </summary>
    [JsonPropertyName("readTime")]
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// Is message deleted on sender side?
    /// </summary>
    [JsonPropertyName("isDeletedOnSenderSide")]
    public bool IsDeletedOnSenderSide { get; set; }
    
    /// <summary>
    /// Is message deleted on receiver side?
    /// </summary>
    [JsonPropertyName("isDeletedOnReceiverSide")]
    public bool IsDeletedOnReceiverSide { get; set; }
}