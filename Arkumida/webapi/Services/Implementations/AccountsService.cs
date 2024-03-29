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

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using webapi.Constants;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Creatures;
using webapi.Models.Creatures.Critics;
using webapi.Models.Enums;
using webapi.Models.Settings;
using webapi.OpenSearch.Models;
using webapi.OpenSearch.Services.Abstract;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Email;

namespace webapi.Services.Implementations;

public class AccountsService : IAccountsService
{
    private readonly UserManager<CreatureDbo> _userManager;
    private readonly ICreaturesMapper _creaturesMapper;
    private readonly IAvatarsMapper _avatarsMapper;
    private readonly IProfilesDao _profilesDao;
    private readonly IAvatarsDao _avatarsDao;
    private readonly ICreaturesWithProfilesMapper _creaturesWithProfilesMapper;
    private readonly IFilesDao _filesDao;
    private readonly IEmailSenderService _emailSenderService;
    private readonly IEmailsGeneratorService _emailsGeneratorService;
    private readonly JwtSettings _jwtSettings;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IArkumidaOpenSearchClient _arkumidaOpenSearchClient;

    public AccountsService
    (
        UserManager<CreatureDbo> userManager,
        ICreaturesMapper creaturesMapper,
        IAvatarsMapper avatarsMapper,
        IProfilesDao profilesDao,
        IAvatarsDao avatarsDao,
        ICreaturesWithProfilesMapper creaturesWithProfilesMapper,
        IFilesDao filesDao,
        IEmailSenderService emailSenderService,
        IEmailsGeneratorService emailsGeneratorService,
        IOptions<JwtSettings> jwtSettings,
        RoleManager<IdentityRole<Guid>> roleManager,
        IArkumidaOpenSearchClient arkumidaOpenSearchClient)
    {
        _userManager = userManager;
        _creaturesMapper = creaturesMapper;
        _avatarsMapper = avatarsMapper;
        _profilesDao = profilesDao;
        _avatarsDao = avatarsDao;
        _creaturesWithProfilesMapper = creaturesWithProfilesMapper;
        _filesDao = filesDao;
        _emailSenderService = emailSenderService;
        _emailsGeneratorService = emailsGeneratorService;
        _jwtSettings = jwtSettings.Value;
        _roleManager = roleManager;
        _arkumidaOpenSearchClient = arkumidaOpenSearchClient;
    }

    public async Task<RegistrationResultDto> RegisterUserAsync(RegistrationDataDto registrationData, bool isImporting)
    {
        _ = registrationData ?? throw new ArgumentNullException(nameof(registrationData), "Registration data must not be null!");
        
        if (await IsUserExistByLoginAsync(registrationData.Login))
        {
            return new RegistrationResultDto(Guid.Empty, UserRegistrationResult.LoginIsTaken);
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
        
        var creature = _creaturesMapper.Map(creatureDbo);

        // Each creature is user
        await AddCreatureToRoleAsync(creature.Id, RolesConstants.UserRole);
        
        // Now creating the profile
        var creatureProfileDbo = new CreatureProfileDbo()
        {
            Id = creature.Id,
            DisplayName = creature.Login,
            
            IsPasswordChangeRequired = isImporting,
            OneTimePlaintextPassword = isImporting ? registrationData.Password : string.Empty,
            
            Avatars = new List<AvatarDbo>(),
            CurrentAvatar = null,
            About = string.Empty
        };

        await _profilesDao.CreateProfileAsync(creatureProfileDbo);
        
        // Adding creature to OpenSearch
        var indexableCreature = new IndexableCreature()
        {
            DbId = creature.Id,
            DisplayName = creatureProfileDbo.DisplayName
        };
        await _arkumidaOpenSearchClient.IndexCreatureAsync(indexableCreature);
        
        return new RegistrationResultDto(creature.Id, UserRegistrationResult.OK);
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
  
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));  
  
        var token = new JwtSecurityToken(
            _jwtSettings.ValidIssuer,
            _jwtSettings.ValidAudience,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.Lifetime),  
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

        if (string.IsNullOrWhiteSpace(email))
        {
            // It is possible that many users will empty emails
            return false;
        }
        
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
            throw new InvalidOperationException($"Creature with ID={creatureId} doesn't exist.");
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
            throw new ArgumentException("Avatar displayName must be populated!", nameof(newName));
        }
        
        var profile = await _profilesDao.GetProfileAsync(creatureId);
        if (profile == null)
        {
            throw new InvalidOperationException($"Creature with ID={creatureId} doesn't exist.");
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

    public async Task DeleteAvatarAsync(Guid creatureId, Guid avatarId)
    {
        var profile = await _profilesDao.GetProfileAsync(creatureId);
        if (profile == null)
        {
            throw new InvalidOperationException($"Creature with ID={creatureId} doesn't exist.");
        }
        
        // Is it our avatar?
        var avatarToUpdate = profile.Avatars.SingleOrDefault(a => a.Id == avatarId);
        if (avatarToUpdate == null)
        {
            throw new ArgumentException($"Avatar with ID={avatarId} doesn't belong to creature with ID={creatureId}.", nameof(avatarId));
        }
        
        // Is it current avatar?
        if (profile.CurrentAvatar?.Id == avatarId)
        {
            await SetCurrentAvatarAsync(creatureId, null); // Switching to "no avatar"
            
            profile = await _profilesDao.GetProfileAsync(creatureId); // Reloading because it is possible that profile was changed due to switching to 
        }
        
        // Removing from profile
        profile.Avatars = profile
            .Avatars
            .Where(a => a.Id != avatarId)
            .ToList();

        await _profilesDao.UpdateProfileAsync(profile);

        var avatar = await _avatarsDao.GetAvatarByIdAsync(avatarId);
        
        // Deleting avatar
        await _avatarsDao.DeleteAvatarAsync(avatarId);
        
        // And avatar file
        await _filesDao.DeleteFileAsync(avatar.File.Id);
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

    public async Task<IDictionary<Guid, CreatureWithProfile>> MassGetProfilesByCreaturesIdsAsync(IReadOnlyCollection<Guid> creaturesIds)
    {
        var distinctCreaturesIds = creaturesIds
            .Distinct()
            .ToList();

        var creatures = new List<CreatureDbo>();
        foreach (var distinctCreatureId in distinctCreaturesIds)
        {
            creatures.Add(await _userManager.FindByIdAsync(distinctCreatureId.ToString()));
        }
        
        var profiles = await _profilesDao.MassGetProfilesAsync(distinctCreaturesIds);

        var creaturesWithProfiles = creatures
            .Select(c => _creaturesWithProfilesMapper.Map(c, profiles.Single(p => p.Id == c.Id)));

        return distinctCreaturesIds
            .ToDictionary(c => c, c => creaturesWithProfiles.SingleOrDefault(cwp => cwp.Id == c));
    }

    public async Task RenameCreatureAsync(Guid creatureId, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Creature's new displayName must not be empty.", nameof(newName));
        }
        
        var profile = await _profilesDao.GetProfileAsync(creatureId);
        if (profile == null)
        {
            throw new InvalidOperationException($"Creature with ID={creatureId} doesn't exist.");
        }

        profile.DisplayName = newName;

        await _profilesDao.UpdateProfileAsync(profile);

        var indexableCreature = new IndexableCreature()
        {
            DbId = creatureId,
            DisplayName = newName
        };

        await _arkumidaOpenSearchClient.UpdateCreatureAsync(indexableCreature);
    }

    public async Task UpdateAboutAsync(Guid creatureId, string newAbout)
    {
        _ = newAbout ?? throw new ArgumentException("Creature's new about must not be null.", nameof(newAbout));
        
        var profile = await _profilesDao.GetProfileAsync(creatureId);
        if (profile == null)
        {
            throw new InvalidOperationException($"Creature with ID={creatureId} doesn't exist.");
        }

        profile.About = newAbout;

        await _profilesDao.UpdateProfileAsync(profile);
    }

    public async Task<bool> ChangePasswordAsync(Guid creatureId, string oldPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(oldPassword))
        {
            throw new ArgumentException("Old password must be non-empty.", nameof(oldPassword));
        }
        
        if (string.IsNullOrWhiteSpace(oldPassword))
        {
            throw new ArgumentException("New password must be non-empty.", nameof(newPassword));
        }

        var user = await _userManager.FindByIdAsync(creatureId.ToString());
        if (user == null)
        {
            throw new InvalidOperationException($"Creature with ID={ creatureId } is not found.");
        }
        
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        
        // Clearing one-time password if set
        if (result.Succeeded)
        {
            var profile = await _profilesDao.GetProfileAsync(creatureId);
            if (profile == null)
            {
                throw new InvalidOperationException($"Creature with ID={creatureId} doesn't exist.");
            }

            if (profile.IsPasswordChangeRequired)
            {
                profile.IsPasswordChangeRequired = false;
                profile.OneTimePlaintextPassword = string.Empty;
                
                await _profilesDao.UpdateProfileAsync(profile);   
            }
        }
        
        return result.Succeeded;
    }

    public async Task<bool> IsCreatureEmailConfirmedAsync(Guid creatureId)
    {
        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Creature with ID={creatureId} is not found!", nameof(creatureId));
        }

        return creature.EmailConfirmed;
    }

    public async Task<bool> InitiateEmailConfirmationAsync(Guid creatureId)
    {
        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Creature with ID={creatureId} is not found!", nameof(creatureId));
        }

        if (creature.EmailConfirmed)
        {
            return false; // Already confirmed
        }
        
        var tokenAsBase64 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(creature)));

        var creatureWithProfile = await GetProfileByCreatureIdAsync(creatureId);
        
        var message = await _emailsGeneratorService.GenerateEmailAddressConfirmationEmailAsync(creatureWithProfile, tokenAsBase64);

        return await _emailSenderService.SendAsync(message, new CancellationToken());
    }

    public async Task<bool> ConfirmEmailAsync(Guid creatureId, string token)
    {
        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Creature with ID={creatureId} is not found!", nameof(creatureId));
        }

        if (creature.EmailConfirmed)
        {
            return false; // Already confirmed
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        return (await _userManager.ConfirmEmailAsync(creature, decodedToken)).Succeeded;
    }

    public async Task<Tuple<bool, bool>> InitiateEmailChangeAsync(Guid creatureId, string newEmail)
    {
        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Creature with ID={creatureId} is not found!", nameof(creatureId));
        }

        if (newEmail == string.Empty)
        {
            // Empty email, just setting it
            var result = await _userManager.SetEmailAsync(creature, "");
            return new Tuple<bool, bool>(result.Succeeded, false);
        }
        
        // We need to send an email confirmation message
        var tokenAsBase64 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GenerateChangeEmailTokenAsync(creature, newEmail)));
        
        var creatureWithProfile = await GetProfileByCreatureIdAsync(creatureId);
        var message = await _emailsGeneratorService.GenerateEmailAddressChangeEmailAsync(creatureWithProfile, newEmail, tokenAsBase64);

        return new Tuple<bool, bool>(await _emailSenderService.SendAsync(message, new CancellationToken()), true);
    }

    public async Task<bool> ChangeEmailAsync(Guid creatureId, string encodedEmail, string token)
    {
        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Creature with ID={creatureId} is not found!", nameof(creatureId));
        }
        
        var decodedEmail = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedEmail));
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        return (await _userManager.ChangeEmailAsync(creature, decodedEmail, decodedToken)).Succeeded;
    }

    public async Task<PasswordResetInitiationResult> InitiatePasswordResetAsync(string login)
    {
        var creature = await _userManager.FindByNameAsync(login);
        if (creature == null)
        {
            return PasswordResetInitiationResult.CreatureNotFound;
        }

        if (string.IsNullOrWhiteSpace(creature.Email))
        {
            // No email at all
            return PasswordResetInitiationResult.CreatureHaveNoEmail;
        }
        
        if (!creature.EmailConfirmed)
        {
            // Creature have no confirmed email, we can't send a link to unconfirmed email
            return PasswordResetInitiationResult.CreatureHaveNoConfirmedEmail;
        }
        
        // Reset token
        var tokenAsBase64 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GeneratePasswordResetTokenAsync(creature)));
        
        // Email
        var creatureWithProfile = await GetProfileByCreatureIdAsync(creature.Id);
        var message = await _emailsGeneratorService.GeneratePasswordResetEmailAsync(creatureWithProfile, tokenAsBase64);
        var isSent = await _emailSenderService.SendAsync(message, new CancellationToken());

        if (!isSent)
        {
            // Failed to send email
            return PasswordResetInitiationResult.FailedToSendEmail;
        }

        return PasswordResetInitiationResult.Initiated;
    }

    public async Task<bool> ResetPasswordAsync(Guid creatureId, string newPassword, string token)
    {
        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Creature with ID={creatureId} is not found!", nameof(creatureId));
        }
        
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        return (await _userManager.ResetPasswordAsync(creature, decodedToken, newPassword)).Succeeded;
    }

    public async Task<IReadOnlyCollection<CreatureWithProfile>> FindCreaturesByDisplayNamePartAsync(string displayNamePart)
    {
        var profiles = await _profilesDao.FindCreaturesProfilesByDisplayNamePartAsync(displayNamePart);

        var result = new List<CreatureWithProfile>();

        foreach (var profile in profiles)
        {
            var creature = await _userManager.FindByIdAsync(profile.Id.ToString());

            result.Add(_creaturesWithProfilesMapper.Map(creature, profile));
        }

        return result;
    }

    public async Task<Creature> FindCreatureByDisplayNameAsync(string displayName)
    {
        var profile = await _profilesDao.FindCreatureByDisplayNameAsync(displayName);

        if (profile == null)
        {
            return null;
        }

        return _creaturesMapper.Map(await _userManager.FindByIdAsync(profile.Id.ToString()));
    }

    public async Task<bool> IsRoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task CreateRoleAsync(string roleName)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to create a role with Name = { roleName }");
        }
    }

    public async Task<bool> IsCreatureInRoleAsync(Guid creatureId, string roleName)
    {
        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Creature with ID = { creatureId } is not found!", nameof(creatureId));
        }

        return await _userManager.IsInRoleAsync(creature, roleName);
    }

    public async Task AddCreatureToRoleAsync(Guid creatureId, string roleNameToAddTo)
    {
        var creature = await _userManager.FindByIdAsync(creatureId.ToString());
        if (creature == null)
        {
            throw new ArgumentException($"Creature with ID = { creatureId } is not found!", nameof(creatureId));
        }
        
        if (!(await _userManager.AddToRoleAsync(creature, roleNameToAddTo)).Succeeded)
        {
            throw new InvalidOperationException($"Failed to add creature with ID = { creatureId } to role { roleNameToAddTo }");
        }
    }

    public async Task<IReadOnlyCollection<Creature>> GetAllCreaturesAsync()
    {
        return _creaturesMapper.Map(await _userManager.Users.ToListAsync());
    }

    public async Task<CriticsSettings> GetCriticsSettingsAsync(Guid creatureId)
    {
        var profile = await _profilesDao.GetProfileAsync(creatureId);
        if (profile == null)
        {
            throw new ArgumentException($"Creature with ID = { creatureId } is not found!", nameof(creatureId));
        }

        return new CriticsSettings()
        {
            IsShowDislikes = profile.IsShowDislikes,
            IsShowDislikesAuthors = profile.IsShowDislikesAuthors
        };
    }

    public async Task<CriticsSettings> UpdateCriticsSettingsAsync(Guid creatureId, CriticsSettings criticsSettings)
    {
        _ = criticsSettings ?? throw new ArgumentNullException(nameof(criticsSettings), "Critics settings mustn't be null!");

        if (criticsSettings.IsShowDislikesAuthors && !criticsSettings.IsShowDislikes)
        {
            throw new ArgumentException("Show dislikes authors must be false if show dislikes is false!", nameof(criticsSettings));
        }
        
        var profile = await _profilesDao.GetProfileAsync(creatureId);
        if (profile == null)
        {
            throw new ArgumentException($"Creature with ID = { creatureId } is not found!", nameof(creatureId));
        }

        profile.IsShowDislikes = criticsSettings.IsShowDislikes;
        profile.IsShowDislikesAuthors = criticsSettings.IsShowDislikesAuthors;

        await _profilesDao.UpdateProfileAsync(profile);

        return criticsSettings;
    }
}