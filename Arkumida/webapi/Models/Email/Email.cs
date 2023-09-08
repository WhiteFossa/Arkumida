namespace webapi.Models.Email;

/// <summary>
/// One email message
/// </summary>
public class Email
{
    // Receiver
    public List<string> To { get; private set; }
    public List<string> Bcc { get; private set; }

    public List<string> Cc { get; private set; }

    // Sender
    public string From { get; private set; }

    public string DisplayName { get; private set; }

    public string ReplyTo { get; private set; }

    public string ReplyToName { get; private set; }

    // Content
    public string Subject { get; private set; }

    public string Body { get; private set; }

    public Email
    (
        List<string> to,
        string subject,
        string body,
        string from = null,
        string displayName = null,
        string replyTo = null,
        string replyToName = null,
        List<string> bcc = null,
        List<string> cc = null
    )
    {
        // Receiver
        To = to;
        Bcc = bcc ?? new List<string>();
        Cc = cc ?? new List<string>();

        // Sender
        From = from;
        DisplayName = displayName;
        ReplyTo = replyTo;
        ReplyToName = replyToName;
            
        // Content
        Subject = subject;
        Body = body;
    }
}