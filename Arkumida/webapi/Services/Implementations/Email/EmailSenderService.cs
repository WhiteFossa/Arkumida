#region License
// Arkumida - Furtails.pw next generation backend
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using webapi.Models.Email;
using webapi.Models.Settings;
using webapi.Services.Abstract.Email;

namespace webapi.Services.Implementations.Email;

public class EmailSenderService : IEmailSenderService
{
    private ILogger _logger;
    private readonly EmailSettings _emailSettings;

    public EmailSenderService
    (
        ILogger<EmailSenderService> logger,
        IOptions<EmailSettings> emailSettings
    )
    {
        _logger = logger;
        _emailSettings = emailSettings.Value;
    }
    
    public async Task<bool> SendAsync(Models.Email.Email email, CancellationToken ct)
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
            else
            {
                throw new InvalidOperationException("Either SSL or StartTLS have to be enabled.");
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
        catch (Exception ex)
        {
            // TODO: Add logging here
            _logger.LogError($"Failed to send email: { ex.Message }");
            return false;
        }
    }
}