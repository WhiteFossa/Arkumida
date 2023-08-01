using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using webapi.Constants;
using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class AccountsService : IAccountsService
{
    private readonly UserManager<UserDbo> _userManager;
    private readonly IConfigurationService _configurationService;
    private readonly IUsersMapper _usersMapper;

    public AccountsService
    (
        UserManager<UserDbo> userManager,
        IConfigurationService configurationService,
        IUsersMapper usersMapper
    )
    {
        _userManager = userManager;
        _configurationService = configurationService;
        _usersMapper = usersMapper;
    }

    public async Task<RegistrationResultDto> RegisterUserAsync(RegistrationDataDto registrationData)
    {
        _ = registrationData ?? throw new ArgumentNullException(nameof(registrationData), "Registration data must not be null!");
        
        if (await IsUserExistByLoginAsync(registrationData.Login))
        {
            return new RegistrationResultDto(Guid.Empty, UserRegistrationResult.LoginIsTaken);
        }

        if (await IsUserExistByEmailAsync(registrationData.Email))
        {
            return new RegistrationResultDto(Guid.Empty, UserRegistrationResult.EmailIsTaken);
        }
        
        var userDbo = new UserDbo()
        {  
            UserName = registrationData.Login,
            Email = registrationData.Email,
            SecurityStamp = Guid.NewGuid().ToString(), // TODO: Is it secure?
            
            // Profile fields
            DisplayName = registrationData.Login
        };  
        
        var result = await _userManager.CreateAsync(userDbo, registrationData.Password);
        if (!result.Succeeded)
        {
            // Mostly probably password is too weak
            return new RegistrationResultDto(Guid.Empty, UserRegistrationResult.WeakPassword);
        }

        var userDto = _usersMapper.Map(userDbo);
        
        return new RegistrationResultDto(userDto.Id, UserRegistrationResult.OK);
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

    public async Task<bool> IsUserExistByLoginAsync(string login)
    {
        _ = login ?? throw new ArgumentNullException(nameof(login), "Login must be specified, at least empty string.");
        
        return (await _userManager.FindByNameAsync(login)) != null;
    }

    public async Task<bool> IsUserExistByEmailAsync(string email)
    {
        _ = email ?? throw new ArgumentNullException(nameof(email), "Email must be specified, at least empty string.");
        
        return (await _userManager.FindByEmailAsync(email)) != null;
    }

    public async Task<User> FindUserByLoginAsync(string login)
    {
        _ = login ?? throw new ArgumentNullException(nameof(login), "Login must be specified, at least empty string.");

        return _usersMapper.Map(await _userManager.FindByNameAsync(login));
    }
}