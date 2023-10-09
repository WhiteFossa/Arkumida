namespace furtails_importer.Dbos;

public class FtPrivateMessage
{
    public int Id { get; set; }
    
    public int Receiver { get; set; }
    
    public int Sender { get; set; }
    
    public DateTime SendTime { get; set; }

    public string Content { get; set; }

    public bool IsRead { get; set; }

    public bool IsDeletedAtReceiver { get; set; }
    
    public bool IsDeletedAtSender { get; set; }

    /// <summary>
    /// 0 - ordinary message, 1 - mistake in text notification, 2 - system notification
    /// </summary>
    public int MessageType { get; set; }

    /// <summary>
    /// Text ID?
    /// </summary>
    public int? MessageTypeTag { get; set; }
}