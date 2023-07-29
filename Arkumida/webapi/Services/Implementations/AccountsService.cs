using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class AccountsService : IAccountsService
{
    public async Task<RegistrationResultDto> RegisterUserAsync(RegistrationDataDto registrationData)
    {
        return new RegistrationResultDto(Guid.NewGuid(), UserRegistrationResult.OK); // TODO: Implement me
    }
}