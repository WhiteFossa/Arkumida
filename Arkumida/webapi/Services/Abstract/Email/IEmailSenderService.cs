namespace webapi.Services.Abstract.Email;

/// <summary>
/// Service to send email
/// </summary>
public interface IEmailSenderService
{
    /// <summary>
    /// Send one email. Returns true if email sent successfully
    /// </summary>
    Task<bool> SendAsync(Models.Email.Email email, CancellationToken ct);
}