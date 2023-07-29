using webapi.Models.Api.DTOs;

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
}