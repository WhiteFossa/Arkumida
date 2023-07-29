using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using webapi.Constants;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Models.Identity;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class AccountsService : IAccountsService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfigurationService _configurationService;

    public AccountsService
    (
        UserManager<User> userManager,
        IConfigurationService configurationService
    )
    {
        _userManager = userManager;
        _configurationService = configurationService;
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
        _ = loginData ?? throw new ArgumentNullException(nameof(loginData), "Login data must not be null.");
        
        var user = await _userManager.FindByNameAsync(loginData.Login);
        if (user == null)
        {
            // User not found
            return new LoginResultDto(false, string.Empty, DateTime.UtcNow);
        }

        if (!await _userManager.CheckPasswordAsync(user, loginData.Password))
        {
            // Password incorrect
            return new LoginResultDto(false, string.Empty, DateTime.UtcNow);
        }
        
        var userRoles = await _userManager.GetRolesAsync(user);  
  
        var authClaims = new List<Claim>  
        {  
            new Claim(ClaimTypes.Name, user.UserName),  
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
        };  
  
        foreach (var userRole in userRoles)  
        {  
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));  
        }  
  
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(await _configurationService.GetConfigurationStringAsync(GlobalConstants.JwtSecretSettingName)));  
  
        var token = new JwtSecurityToken(
            await _configurationService.GetConfigurationStringAsync(GlobalConstants.JwtValidIssuerSettingName),
            await _configurationService.GetConfigurationStringAsync(GlobalConstants.JwtValidAudienceSettingName),
            expires: DateTime.UtcNow.AddHours(await _configurationService.GetConfigurationIntAsync(GlobalConstants.JwtLifetimeSettingName)),  
            claims: authClaims,  
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)  
        );

        return new LoginResultDto(true, new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }
}