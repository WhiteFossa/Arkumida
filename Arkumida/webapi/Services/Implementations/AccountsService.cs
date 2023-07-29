using Microsoft.AspNetCore.Identity;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Models.Identity;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class AccountsService : IAccountsService
{
    private readonly UserManager<User> _userManager;

    public AccountsService
    (
        UserManager<User> userManager
    )
    {
        _userManager = userManager;
    }

    public async Task<RegistrationResultDto> RegisterUserAsync(RegistrationDataDto registrationData)
    {
        _ = registrationData ?? throw new ArgumentNullException(nameof(registrationData), "Registration data must not be null!");
        
        var existingUser = await _userManager.FindByNameAsync(registrationData.Login);
        if (existingUser != null)
        {
            return new RegistrationResultDto(string.Empty, UserRegistrationResult.LoginIsTaken);
        }

        existingUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (existingUser != null)
        {
            return new RegistrationResultDto(string.Empty, UserRegistrationResult.EmailIsTaken);
        }
        
        var user = new User()  
        {  
            UserName = registrationData.Login,
            Email = registrationData.Email,
            SecurityStamp = Guid.NewGuid().ToString() // TODO: Is it secure?
        };  
        
        var result = await _userManager.CreateAsync(user, registrationData.Password);
        if (!result.Succeeded)
        {
            // Mostly probably password is too weak
            return new RegistrationResultDto(string.Empty, UserRegistrationResult.WeakPassword);
        }
        
        return new RegistrationResultDto(user.Id, UserRegistrationResult.OK);
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto loginData)
    {
        return new LoginResultDto(true, "YiffYuff", DateTime.UtcNow);
    }
}