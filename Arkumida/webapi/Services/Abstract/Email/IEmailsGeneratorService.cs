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
    Task<Models.Email.Email> GenerateEmailAddressConfirmationEmail(CreatureWithProfile creatureWithProfile, string confirmationToken);
    
    /// <summary>
    /// Generate an email with email address change link
    /// </summary>
    Task<Models.Email.Email> GenerateEmailAddressChangeEmail(CreatureWithProfile creatureWithProfile, string newEmail, string changeToken);
}