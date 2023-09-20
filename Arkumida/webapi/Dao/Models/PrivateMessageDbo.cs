using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

/// <summary>
/// Database model for private message
/// </summary>
public class PrivateMessageDbo
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Message content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Message sender
    /// </summary>
    public CreatureProfileDbo Sender { get; set; }
    
    /// <summary>
    /// Message receiver
    /// </summary>
    public CreatureProfileDbo Receiver { get; set; }

    /// <summary>
    /// Message was sent at this time
    /// </summary>
    public DateTime SentTime { get; set; }
    
    /// <summary>
    /// Message was read (if was) at this time
    /// </summary>
    public DateTime? ReadTime { get; set; }
}