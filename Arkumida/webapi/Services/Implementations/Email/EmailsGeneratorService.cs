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
    
    public async Task<Models.Email.Email> GenerateEmailAddressConfirmationEmailAsync(CreatureWithProfile creatureWithProfile, string confirmationToken)
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

    public async Task<Models.Email.Email> GenerateEmailAddressChangeEmailAsync(CreatureWithProfile creatureWithProfile, string newEmail, string changeToken)
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

    public async Task<Models.Email.Email> GeneratePasswordResetEmailAsync(CreatureWithProfile creatureWithProfile, string resetToken)
    {
        var template = await File.ReadAllTextAsync("Resources/Email/PasswordResetTemplate.html");

        var body = string.Format
        (
            template,
            creatureWithProfile.DisplayName, // {0}
            _siteInfoSettings.BaseUrl, // {1}
            creatureWithProfile.Id, // {2}
            resetToken // {3}
        );
        
        return new Models.Email.Email
        (
            new List<string>() { creatureWithProfile.Email },
            "Сброс пароля",
            body
        );
    }
}