using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using webapi.Constants;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class AccountsService : IAccountsService
{
    private readonly UserManager<CreatureDbo> _userManager;
    private readonly IConfigurationService _configurationService;
    private readonly ICreaturesMapper _creaturesMapper;
    private readonly IAvatarsMapper _avatarsMapper;
    private readonly IProfilesDao _profilesDao;
    private readonly IAvatarsDao _avatarsDao;
    private readonly ICreaturesWithProfilesMapper _creaturesWithProfilesMapper;

    public AccountsService
    (
        UserManager<CreatureDbo> userManager,
        IConfigurationService configurationService,
        ICreaturesMapper creaturesMapper,
        IAvatarsMapper avatarsMapper,
        IProfilesDao profilesDao,
        IAvatarsDao avatarsDao,
        ICreaturesWithProfilesMapper creaturesWithProfilesMapper
    )
    {
        _userManager = userManager;
        _configurationService = configurationService;
        _creaturesMapper = creaturesMapper;
        _avatarsMapper = avatarsMapper;
        _profilesDao = profilesDao;
        _avatarsDao = avatarsDao;
        _creaturesWithProfilesMapper = creaturesWithProfilesMapper;
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
        
        var creatureDbo = new CreatureDbo()
        {  
            UserName = registrationData.Login,
            Email = registrationData.Email,
            SecurityStamp = Guid.NewGuid().ToString() // TODO: Is it secure?
        };  
        
        var result = await _userManager.CreateAsync(creatureDbo, registrationData.Password);
        if (!result.Succeeded)
        {
            // Mostly probably password is too weak
            return new RegistrationResultDto(Guid.Empty, UserRegistrationResult.WeakPassword);
        }
        
        var creatureDto = _creaturesMapper.Map(creatureDbo);
        
        // Now creating the profile
        var creatureProfileDbo = new CreatureProfileDbo()
        {
            Id = creatureDto.Id,
            DisplayName = creatureDto.Login,
            OneTimePlaintextPassword = registrationData.Password,
            Avatars = new List<AvatarDbo>(),
            CurrentAvatar = null
        };

        await _profilesDao.CreateProfileAsync(creatureProfileDbo);
        
        return new RegistrationResultDto(creatureDto.Id, UserRegistrationResult.OK);
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

    public async Task<Creature> FindUserByLoginAsync(string login)
    {
        _ = login ?? throw new ArgumentNullException(nameof(login), "Login must be specified, at least empty string.");

        return _creaturesMapper.Map(await _userManager.FindByNameAsync(login));
    }

    public async Task<Avatar> AddAvatarAsync(Guid creatureId, Avatar avatar)
    {
        _ = avatar ?? throw new ArgumentNullException(nameof(avatar), "Avatar must be specified");

        var avatarDbo = _avatarsMapper.Map(avatar);

        await _avatarsDao.AddAvatarToCreatureAsync(creatureId, avatarDbo);

        return _avatarsMapper.Map(avatarDbo);
    }

    public async Task SetCurrentAvatarAsync(Guid creatureId, Guid? avatarId)
    {
        var profile = await _profilesDao.GetProfileAsync(creatureId);
        if (profile == null)
        {
            throw new InvalidOperationException($"Profile is not found for existing creature with ID={creatureId}");
        }

        if (!avatarId.HasValue)
        {
            // User choose to not have an avatar
            profile.CurrentAvatar = null;
        }
        else
        {
            // User choose a real avatar
            
            // Is it our avatar?
            if (!profile.Avatars.Any(a => a.Id == avatarId.Value))
            {
                throw new ArgumentException($"Avatar with ID={ avatarId.Value } doesn't belong to creature with ID={ creatureId }.", nameof(avatarId));
            }

            profile.CurrentAvatar = new AvatarDbo() // UpdateProfileAsync() will load avatar by ID
            {
                Id = avatarId.Value
            };
        }
        
        await _profilesDao.UpdateProfileAsync(profile);
    }

    public async Task RenameAvatarAsync(Guid creatureId, Guid avatarId, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Avatar name must be populated!", nameof(newName));
        }
        
        var profile = await _profilesDao.GetProfileAsync(creatureId);
        if (profile == null)
        {
            throw new InvalidOperationException($"Profile is not found for existing creature with ID={creatureId}");
        }
        
        // Is it our avatar?
        var avatarToUpdate = profile.Avatars.SingleOrDefault(a => a.Id == avatarId);
        if (avatarToUpdate == null)
        {
            throw new ArgumentException($"Avatar with ID={avatarId} doesn't belong to creature with ID={creatureId}.", nameof(avatarId));
        }

        avatarToUpdate.Name = newName;

        await _avatarsDao.UpdateAvatarAsync(avatarToUpdate);
    }

    public async Task<CreatureWithProfile> GetProfileByCreatureIdAsync(Guid creatureId)
    {
        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Creature with ID={creatureId} is not found!", nameof(creatureId));
        }

        var profile = await _profilesDao.GetProfileAsync(creatureId);
        if (profile == null)
        {
            throw new InvalidOperationException($"Profile is not found for existing creature with ID={creatureId}");
        }

        return _creaturesWithProfilesMapper.Map(creature, profile);
    }
}