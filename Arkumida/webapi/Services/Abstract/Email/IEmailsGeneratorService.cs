using webapi.Models;

namespace webapi.Services.Abstract.Email;

/// <summary>
/// Service to generate various emails from templates
/// </summary>
public interface IEmailsGeneratorService
{
    /// <summary>
    /// Generate an email with email address confirmation link
    /// </summary>
    Task<Models.Email.Email> GenerateEmailAddressConfirmationEmailAsync(CreatureWithProfile creatureWithProfile, string confirmationToken);
    
    /// <summary>
    /// Generate an email with email address change link
    /// </summary>
    Task<Models.Email.Email> GenerateEmailAddressChangeEmailAsync(CreatureWithProfile creatureWithProfile, string newEmail, string changeToken);

    /// <summary>
    /// Generate an email with link to reset a password
    /// </summary>
    Task<Models.Email.Email> GeneratePasswordResetEmailAsync(CreatureWithProfile creatureWithProfile, string resetToken);
}