using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Requests;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with user accounts
/// </summary>
public interface IAccountsService
{
    /// <summary>
    /// Try to register an user
    /// </summary>
    Task<RegistrationResultDto> RegisterUserAsync(RegistrationDataDto registrationData);

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
    Task SetCurrentAvatarAsync(Guid creatureId, Guid avatarId);

    /// <summary>
    /// Rename creature's avatar
    /// </summary>
    Task RenameAvatarAsync(Guid creatureId, Guid avatarId, string newName);
    
    /// <summary>
    /// Get creature profile by ID
    /// </summary>
    Task<CreatureWithProfile> GetProfileByCreatureIdAsync(Guid creatureId);
}