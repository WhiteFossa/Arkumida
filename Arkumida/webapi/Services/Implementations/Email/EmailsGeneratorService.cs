using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using webapi.Constants;
using webapi.Models;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Email;
using File = System.IO.File;

namespace webapi.Services.Implementations.Email;

public class EmailsGeneratorService : IEmailsGeneratorService
{
    private readonly IConfigurationService _configurationService;

    public EmailsGeneratorService
    (
        IConfigurationService configurationService
    )
    {
        _configurationService = configurationService;
    }
    
    public async Task<Models.Email.Email> GenerateEmailAddressConfirmationEmail(CreatureWithProfile creatureWithProfile, string confirmationToken)
    {
        var template = await File.ReadAllTextAsync("Resources/Email/EmailAddressConfirmationTemplate.html");

        var siteBaseUrl = await _configurationService.GetConfigurationStringAsync(GlobalConstants.SiteInfoBaseUrlSettingName);

        var body = string.Format
        (
            template,
            creatureWithProfile.DisplayName, // {0}
            creatureWithProfile.Email, // {1}
            siteBaseUrl, // {2}
            creatureWithProfile.Id, // {3}
            confirmationToken // {4}
        );
        
        return new Models.Email.Email
        (
            new List<string>() { creatureWithProfile.Email },
            "Подтверждение адреса электронной почты",
            body
        );
    }

    public async Task<Models.Email.Email> GenerateEmailAddressChangeEmail(CreatureWithProfile creatureWithProfile, string newEmail, string changeToken)
    {
        var template = await File.ReadAllTextAsync("Resources/Email/EmailAddressChangeTemplate.html");

        var siteBaseUrl = await _configurationService.GetConfigurationStringAsync(GlobalConstants.SiteInfoBaseUrlSettingName);
        var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(newEmail));

        var body = string.Format
        (
            template,
            creatureWithProfile.DisplayName, // {0}
            newEmail, // {1}
            siteBaseUrl, // {2}
            creatureWithProfile.Id, // {3}
            encodedEmail, // {4}
            changeToken // {5}
        );
        
        return new Models.Email.Email
        (
            new List<string>() { newEmail },
            "Изменение адреса электронной почты",
            body
        );
    }
}