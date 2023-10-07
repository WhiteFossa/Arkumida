using webapi.Models.Api.DTOs.PrivateMessages;

namespace webapi.Models.PrivateMessages;

/// <summary>
/// Private message (business logic level model)
/// </summary>
public class PrivateMessage
{
    /// <summary>
    /// ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Message content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Message sender
    /// </summary>
    public Creature Sender { get; set; }
    
    /// <summary>
    /// Message receiver
    /// </summary>
    public Creature Receiver { get; set; }

    /// <summary>
    /// Message was sent at this time
    /// </summary>
    public DateTime SentTime { get; set; }
    
    /// <summary>
    /// Message was read (if was) at this time
    /// </summary>
    public DateTime? ReadTime { get; set; }
    
    /// <summary>
    /// Is message received on receiver side
    /// </summary>
    public bool IsDeletedOnReceiverSide { get; set; }
    
    /// <summary>
    /// Is message received on receiver side
    /// </summary>
    public bool IsDeletedOnSenderSide { get; set; }

    public PrivateMessageDto ToDto()
    {
        return new PrivateMessageDto(Id, Content, Sender.ToDto(), Receiver.ToDto(), SentTime, ReadTime);
    }
}