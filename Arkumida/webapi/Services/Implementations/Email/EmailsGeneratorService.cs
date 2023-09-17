using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using webapi.Constants;
using webapi.Models;
using webapi.Models.Settings;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Email;
using File = System.IO.File;

namespace webapi.Services.Implementations.Email;

public class EmailsGeneratorService : IEmailsGeneratorService
{
    private readonly SiteInfoSettings _siteInfoSettings;

    public EmailsGeneratorService
    (
        IOptions<SiteInfoSettings> siteInfoSettings)
    {
        _siteInfoSettings = siteInfoSettings.Value;
    }
    
    public async Task<Models.Email.Email> GenerateEmailAddressConfirmationEmail(CreatureWithProfile creatureWithProfile, string confirmationToken)
    {
        var template = await File.ReadAllTextAsync("Resources/Email/EmailAddressConfirmationTemplate.html");

        var body = string.Format
        (
            template,
            creatureWithProfile.DisplayName, // {0}
            creatureWithProfile.Email, // {1}
            _siteInfoSettings.BaseUrl, // {2}
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
        
        var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(newEmail));

        var body = string.Format
        (
            template,
            creatureWithProfile.DisplayName, // {0}
            newEmail, // {1}
            _siteInfoSettings.BaseUrl, // {2}
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