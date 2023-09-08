namespace webapi.Models.Email;

/// <summary>
/// Settings for sending emails
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Sender address
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Sender name
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// SMTP server address
    /// </summary>
    public string SmtpHost { get; set; }

    /// <summary>
    /// SMTP server port
    /// </summary>
    public int SmtpPort { get; set; }

    /// <summary>
    /// Use SSL for SMTP connection
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// Use StartTLS for SMTP connection
    /// </summary>
    public bool UseStartTls { get; set; }

    /// <summary>
    /// SMTP server username
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// SMTP server password
    /// </summary>
    public string Password { get; set; }
}