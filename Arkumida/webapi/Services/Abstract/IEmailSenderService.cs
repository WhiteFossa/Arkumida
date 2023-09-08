using webapi.Models.Email;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to send email
/// </summary>
public interface IEmailSenderService
{
    /// <summary>
    /// Send one email. Returns true if email sent successfully
    /// </summary>
    Task<bool> SendAsync(Email email, CancellationToken ct);
}