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
}