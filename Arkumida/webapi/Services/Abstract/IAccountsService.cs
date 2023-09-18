using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Requests;
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
    /// Change creature's display name
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
}