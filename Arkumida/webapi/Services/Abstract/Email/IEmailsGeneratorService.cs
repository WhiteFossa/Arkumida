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

using webapi.Models;
using webapi.Models.Creatures;

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