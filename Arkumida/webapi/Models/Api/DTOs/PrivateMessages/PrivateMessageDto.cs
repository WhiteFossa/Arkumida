namespace webapi.Models.Api.DTOs.PrivateMessages;

/// <summary>
/// Private message DTO
/// </summary>
public class PrivateMessageDto
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
    public CreatureDto Sender { get; set; }
    
    /// <summary>
    /// Message receiver
    /// </summary>
    public CreatureDto Receiver { get; set; }

    /// <summary>
    /// Message was sent at this time
    /// </summary>
    public DateTime SentTime { get; set; }
    
    /// <summary>
    /// Message was read (if was) at this time
    /// </summary>
    public DateTime? ReadTime { get; set; }

    public PrivateMessageDto
    (
        Guid id,
        string content,
        CreatureDto sender,
        CreatureDto receiver,
        DateTime sentTime,
        DateTime? readTime
    )
    {
        Id = id;

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Private message content can't be empty!", nameof(content));
        }

        Content = content;

        Sender = sender ?? throw new ArgumentNullException(nameof(sender), "Sender can't be null!");
        Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver), "Receiver can't be null!");

        SentTime = sentTime;
        ReadTime = readTime;
    }
}