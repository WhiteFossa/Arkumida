using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using webapi.Models.Email;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class EmailSenderService : IEmailSenderService
{
    /// <summary>
    /// Email settings
    /// </summary>
    private readonly EmailSettings _emailSettings;

    public EmailSenderService
    (
        IOptions<EmailSettings> emailSettings
    )
    {
        _emailSettings = emailSettings.Value;
    }
    
    public async Task<bool> SendAsync(Email email, CancellationToken ct)
    {
        try
        {
            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            #region Sender / Receiver
            
            // Sender
            mail.From.Add(new MailboxAddress(_emailSettings.DisplayName, email.From ?? _emailSettings.From));
            mail.Sender = new MailboxAddress(email.DisplayName ?? _emailSettings.DisplayName, email.From ?? _emailSettings.From);

            // Receiver
            foreach (string mailAddress in email.To)
            {
                mail.To.Add(MailboxAddress.Parse(mailAddress));
            }

            // Set Reply to if specified in mail data
            if (!string.IsNullOrEmpty(email.ReplyTo))
            {
                mail.ReplyTo.Add(new MailboxAddress(email.ReplyToName, email.ReplyTo));
            }

            // BCC
            // Check if a BCC was supplied in the request
            if (email.Bcc != null)
            {
                // Get only addresses where value is not null or with whitespace. x = value of address
                foreach (string mailAddress in email.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }
            }

            // CC
            // Check if a CC address was supplied in the request
            if (email.Cc != null)
            {
                foreach (string mailAddress in email.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }
            }
            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = email.Subject;
            body.HtmlBody = email.Body;
            mail.Body = body.ToMessageBody();

            #endregion

            #region Send Mail

            using var smtp = new SmtpClient();

            if (_emailSettings.UseSsl)
            {
                await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.SslOnConnect, ct);
            }
            else if (_emailSettings.UseStartTls)
            {
                await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls, ct);
            }

            if (!string.IsNullOrEmpty(_emailSettings.UserName))
            {
                await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password, ct);
            }
            
            await smtp.SendAsync(mail, ct);
            await smtp.DisconnectAsync(true, ct);
            
            #endregion

            return true;

        }
        catch (Exception)
        {
            return false;
        }
    }
}