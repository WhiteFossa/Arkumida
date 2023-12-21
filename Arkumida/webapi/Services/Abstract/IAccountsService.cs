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
using webapi.Models.Api.DTOs;
using webapi.Models.Creatures;
using webapi.Models.Creatures.Critics;
using webapi.Models.Enums;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with user accounts
/// </summary>
public interface IAccountsService
{
    /// <summary>
    /// Try to register an user.
    /// If isImporting == true, then we are creating a creature by furtails-importer, so we will store the password and will require to change it
    /// </summary>
    Task<RegistrationResultDto> RegisterUserAsync(RegistrationDataDto registrationData, bool isImporting);

    /// <summary>
    /// Try to log-in user
    /// </summary>
    Task<LoginResultDto> LoginAsync(LoginDto loginData);

    /// <summary>
    /// Is user with given login exists?
    /// </summary>
    Task<bool> IsUserExistByLoginAsync(string login);

    /// <summary>
    /// Is user with given email exists?
    /// </summary>
    Task<bool> IsUserExistByEmailAsync(string email);

    /// <summary>
    /// Find user by login. Returns null if user is not found
    /// </summary>
    Task<Creature> FindUserByLoginAsync(string login);

    /// <summary>
    /// Add new avatar to creature's collection
    /// </summary>
    Task<Avatar> AddAvatarAsync(Guid creatureId, Avatar avatar);

    /// <summary>
    /// Set current avatar for creature
    /// </summary>
    Task SetCurrentAvatarAsync(Guid creatureId, Guid? avatarId);

    /// <summary>
    /// Rename creature's avatar
    /// </summary>
    Task RenameAvatarAsync(Guid creatureId, Guid avatarId, string newName);
    
    /// <summary>
    /// Delete creature's avatar
    /// </summary>
    Task DeleteAvatarAsync(Guid creatureId, Guid avatarId);
    
    /// <summary>
    /// Get creature profile by ID
    /// </summary>
    Task<CreatureWithProfile> GetProfileByCreatureIdAsync(Guid creatureId);
    
    /// <summary>
    /// Mass get creatures by guids list. If creature with given ID is not found, then resulting pair will have null in value.
    /// </summary>
    Task<IDictionary<Guid, CreatureWithProfile>> MassGetProfilesByCreaturesIdsAsync(IReadOnlyCollection<Guid> creaturesIds);

    /// <summary>
    /// Change creature's display displayName
    /// </summary>
    Task RenameCreatureAsync(Guid creatureId, string newName);
    
    /// <summary>
    /// Update creature's about information
    /// </summary>
    Task UpdateAboutAsync(Guid creatureId, string newAbout);

    /// <summary>
    /// Change creature's password
    /// </summary>
    Task<bool> ChangePasswordAsync(Guid creatureId, string oldPassword, string newPassword);

    /// <summary>
    /// Returns true if creature's email is confirmed
    /// </summary>
    Task<bool> IsCreatureEmailConfirmedAsync(Guid creatureId);

    /// <summary>
    /// Initiate email confirmation process for creature. Returns true if email with confirmation link is sent successfully 
    /// </summary>
    Task<bool> InitiateEmailConfirmationAsync(Guid creatureId);

    /// <summary>
    /// Confirm creature's email
    /// </summary>
    Task<bool> ConfirmEmailAsync(Guid creatureId, string token);

    /// <summary>
    /// Change creature's email. Please note that email can be empty string (in this case it's updated immediately and no confirmation email is being sent)
    /// </summary>
    /// <returns>Item1 - is successful, Item2 - is email confirmation required</returns>
    Task<Tuple<bool, bool>> InitiateEmailChangeAsync(Guid creatureId, string newEmail);

    /// <summary>
    /// Complete email change. Please note that email is encoded as BASE64
    /// </summary>
    Task<bool> ChangeEmailAsync(Guid creatureId, string encodedEmail, string token);

    /// <summary>
    /// Initiate creature's password reset
    /// </summary>
    Task<PasswordResetInitiationResult> InitiatePasswordResetAsync(string login);

    /// <summary>
    /// Reset creature's password
    /// </summary>
    Task<bool> ResetPasswordAsync(Guid creatureId, string newPassword, string token);

    /// <summary>
    /// Find all creatures, who's display names contains displayNamePart (case insensitive)
    /// </summary>
    Task<IReadOnlyCollection<CreatureWithProfile>> FindCreaturesByDisplayNamePartAsync(string displayNamePart);

    /// <summary>
    /// Find creature by exact display displayName match
    /// </summary>
    Task<Creature> FindCreatureByDisplayNameAsync(string displayName);

    /// <summary>
    /// Is role exists?
    /// </summary>
    Task<bool> IsRoleExistsAsync(string roleName);

    /// <summary>
    /// Creates a new role (without checks, checks for role existence by hirself, please)
    /// </summary>
    Task CreateRoleAsync(string roleName);

    /// <summary>
    /// Is creature in given role?
    /// </summary>
    Task<bool> IsCreatureInRoleAsync(Guid creatureId, string roleName);

    /// <summary>
    /// Adds creature to a role
    /// </summary>
    Task AddCreatureToRoleAsync(Guid creatureId, string roleNameToAddTo);

    /// <summary>
    /// Lists all registered creatures including service accounts like Importer
    /// </summary>
    Task<IReadOnlyCollection<Creature>> GetAllCreaturesAsync();

    /// <summary>
    /// Get critics settings for given creature
    /// </summary>
    Task<CriticsSettings> GetCriticsSettingsAsync(Guid creatureId);

    /// <summary>
    /// Update critics settings for given creature
    /// </summary>
    Task<CriticsSettings> UpdateCriticsSettingsAsync(Guid creatureId, CriticsSettings criticsSettings);
}