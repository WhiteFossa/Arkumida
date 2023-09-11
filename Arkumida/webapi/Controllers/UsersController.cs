using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Constants;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Requests;
using webapi.Models.Api.Requests.Creature;
using webapi.Models.Api.Responses;
using webapi.Models.Api.Responses.Creature;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with users
/// </summary>
[Authorize]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAccountsService _accountsService;
    private readonly ITextUtilsService _textUtilsService;
    private readonly IConfigurationService _configurationService;

    public UsersController
    (
        IAccountsService accountsService,
        ITextUtilsService textUtilsService,
        IConfigurationService configurationService
    )
    {
        _accountsService = accountsService;
        _textUtilsService = textUtilsService;
        _configurationService = configurationService;
    }

    /// <summary>
    /// Try to register a new user
    /// </summary>
    [AllowAnonymous]
    [Route("api/Users/Register")]
    [HttpPost]
    public async Task<ActionResult<UserRegistrationResponse>> RegisterAsync([FromBody] UserRegistrationRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request must be provided.");
        }

        if (request.RegistrationData == null)
        {
            return BadRequest("Registration data must not be null.");
        }
        
        // Is importing user?
        var isImporting = User.Identity.Name == await _configurationService.GetConfigurationStringAsync(GlobalConstants.ImporterUserLoginSettingName);

        var registrationResult = await _accountsService.RegisterUserAsync(request.RegistrationData, isImporting);

        return Ok(new UserRegistrationResponse(registrationResult));
    }
    
    /// <summary>
    /// Logging in
    /// </summary>
    [AllowAnonymous]
    [HttpPost]  
    [Route("api/Users/Login")]  
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request)  
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (request.LoginData == null)
        {
            return BadRequest("Login data must not be null.");
        }

        var result = await _accountsService.LoginAsync(request.LoginData);
        
        return Ok(new LoginResponse(result));  
    }

    /// <summary>
    /// Check if login is taken
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    [Route("api/Users/IsLoginTaken")]
    public async Task<ActionResult<CheckIfLoginTakenResponse>> IsLoginTakenAsync([FromBody] CheckIfLoginTakenRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (request.CheckData == null)
        {
            return BadRequest("Check data must not be null.");
        }

        var isTaken = await _accountsService.IsUserExistByLoginAsync(request.CheckData.Login);

        return Ok(new CheckIfLoginTakenResponse(new CheckIfLoginTakenResultDto(isTaken)));
    }
    
    /// <summary>
    /// Check if email is taken
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    [Route("api/Users/IsEmailTaken")]
    public async Task<ActionResult<CheckIfEmailTakenResponse>> IsEmailTakenAsync([FromBody] CheckIfEmailTakenRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (request.CheckData == null)
        {
            return BadRequest("Check data must not be null.");
        }

        var isTaken = await _accountsService.IsUserExistByEmailAsync(request.CheckData.Email);

        return Ok(new CheckIfEmailTakenResponse(new CheckIfEmailTakenResultDto(isTaken)));
    }

    /// <summary>
    /// Try to find creature by login
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    [Route("api/Users/FindByLogin")]
    public async Task<ActionResult<FindCreatureByLoginResponse>> FindByLoginAsync([FromBody] FindCreatureByLoginRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (request.SearchData == null)
        {
            return BadRequest("Search data must not be null.");
        }

        var user = await _accountsService.FindUserByLoginAsync(request.SearchData.Login);
        if (user == null)
        {
            // Not found
            return Ok(new FindCreatureByLoginResponse(false, null));
        }
        
        return Ok(new FindCreatureByLoginResponse(true, user.ToDto()));
    }

    /// <summary>
    /// Get current logged-in user
    /// </summary>
    [HttpGet]
    [Route("api/Users/Current")]
    public async Task<ActionResult<LoggedInCreatureResponse>> GetLoggedInCreatureAsync()
    {
        var creature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);
        if (creature == null)
        {
            throw new Exception("Current creature is not found. This is impossible, there is a bug somewhere!");
        }

        return Ok(new LoggedInCreatureResponse(creature.ToDto()));
    }

    /// <summary>
    /// Create creature's avatar
    /// </summary>
    [HttpPost]
    [Route("api/Users/{creatureId}/CreateAvatar")]
    public async Task<ActionResult<CreateAvatarResponse>> CreateAvatarAsync(Guid creatureId, CreateAvatarRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (request.Avatar == null)
        {
            return BadRequest("Avatar must be specified.");
        }

        await CheckPrivilegesAsync(creatureId);
        
        var createdAvatar = await _accountsService.AddAvatarAsync(creatureId, request.Avatar.ToModel());

        return Ok(new CreateAvatarResponse(createdAvatar.ToDto()));
    }

    /// <summary>
    /// Set creature's current avatar
    /// </summary>
    [HttpPost]
    [Route("api/Users/{creatureId}/SetCurrentAvatar")]
    public async Task<ActionResult<CreatureWithProfileResponse>> SetCurrentAvatarAsync(Guid creatureId, SetCurrentAvatarRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        await CheckPrivilegesAsync(creatureId);
        
        await _accountsService.SetCurrentAvatarAsync(creatureId, request.AvatarId);
        
        var profile = await _accountsService.GetProfileByCreatureIdAsync(creatureId);

        return Ok(new CreatureWithProfileResponse(profile.ToDto()));
    }
    
    /// <summary>
    /// Rename creature's avatar
    /// </summary>
    [HttpPost]
    [Route("api/Users/{creatureId}/RenameAvatar")]
    public async Task<ActionResult<CreatureWithProfileResponse>> RenameAvatarAsync(Guid creatureId, RenameAvatarRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        await CheckPrivilegesAsync(creatureId);
        
        await _accountsService.RenameAvatarAsync(creatureId, request.AvatarId, request.NewName);
        
        var profile = await _accountsService.GetProfileByCreatureIdAsync(creatureId);

        return Ok(new CreatureWithProfileResponse(profile.ToDto()));
    }
    
    /// <summary>
    /// Delete creature's avatar
    /// </summary>
    [HttpPost]
    [Route("api/Users/{creatureId}/DeleteAvatar")]
    public async Task<ActionResult<CreatureWithProfileResponse>> DeleteAvatarAsync(Guid creatureId, DeleteAvatarRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        await CheckPrivilegesAsync(creatureId);
        
        await _accountsService.DeleteAvatarAsync(creatureId, request.AvatarId);
        
        var profile = await _accountsService.GetProfileByCreatureIdAsync(creatureId);

        return Ok(new CreatureWithProfileResponse(profile.ToDto()));
    }

    /// <summary>
    /// Get creature's profile
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    [Route("api/Users/{creatureId}/Profile")]
    public async Task<ActionResult<CreatureWithProfileResponse>> GetProfileAsync(Guid creatureId)
    {
        var profile = await _accountsService.GetProfileByCreatureIdAsync(creatureId);

        return Ok(new CreatureWithProfileResponse(profile.ToDto()));
    }
    
    /// <summary>
    /// Change creature's display name
    /// </summary>
    [HttpPost]
    [Route("api/Users/{creatureId}/Rename")]
    public async Task<ActionResult<CreatureWithProfileResponse>> RenameAsync(Guid creatureId, RenameCreatureRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        await CheckPrivilegesAsync(creatureId);

        await _accountsService.RenameCreatureAsync(creatureId, request.NewName);
        
        var profile = await _accountsService.GetProfileByCreatureIdAsync(creatureId);

        return Ok(new CreatureWithProfileResponse(profile.ToDto()));
    }
    
    /// <summary>
    /// Change creature's about information
    /// </summary>
    [HttpPost]
    [Route("api/Users/{creatureId}/About/Update")]
    public async Task<ActionResult<CreatureWithProfileResponse>> UpdateAboutAsync(Guid creatureId, UpdateAboutInfoRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        await CheckPrivilegesAsync(creatureId);

        await _accountsService.UpdateAboutAsync(creatureId, request.NewAbout);
        
        var profile = await _accountsService.GetProfileByCreatureIdAsync(creatureId);

        return Ok(new CreatureWithProfileResponse(profile.ToDto()));
    }

    /// <summary>
    /// Get creature's about info as elements
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    [Route("api/Users/{creatureId}/About/GetAsElements")]
    public async Task<ActionResult<AboutInfoAsElementsResponse>> GetAboutAsElementsAsync(Guid creatureId)
    {
        var profile = await _accountsService.GetProfileByCreatureIdAsync(creatureId);

        var aboutAsElements = _textUtilsService.ParseTextToElements(profile.About, new List<TextFile>()); // About info have no files

        return Ok(new AboutInfoAsElementsResponse(aboutAsElements));
    }
    
    /// <summary>
    /// Change creature's password
    /// </summary>
    [HttpPost]
    [Route("api/Users/{creatureId}/ChangePassword")]
    public async Task<ActionResult<ChangePasswordResponse>> ChangePasswordAsync(Guid creatureId, ChangePasswordRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        await CheckPrivilegesAsync(creatureId);

        var isSuccessful = await _accountsService.ChangePasswordAsync(creatureId, request.OldPassword, request.NewPassword);

        return Ok(new ChangePasswordResponse(isSuccessful));
    }

    /// <summary>
    /// Checks if creature have confirmed email
    /// </summary>
    [HttpGet]
    [Route("api/Users/{creatureId}/Email/IsConfirmed")]
    public async Task<ActionResult<IsEmailConfirmedResponse>> IsEmailConfirmedAsync(Guid creatureId)
    {
        await CheckPrivilegesAsync(creatureId);

        var isConfirmed = await _accountsService.IsCreatureEmailConfirmedAsync(creatureId);

        return Ok(new IsEmailConfirmedResponse(isConfirmed));
    }

    /// <summary>
    /// Initiate email confirmation process
    /// </summary>
    [HttpPost]
    [Route("api/Users/{creatureId}/Email/InitiateConfirmation")]
    public async Task<ActionResult<InitiateEmailConfirmationResponse>> InitiateEmailConfirmationAsync(Guid creatureId)
    {
        await CheckPrivilegesAsync(creatureId);

        if (await _accountsService.IsCreatureEmailConfirmedAsync(creatureId))
        {
            // Already confirmed
            return Ok(new InitiateEmailConfirmationResponse(false));
        }
        
        return Ok(new InitiateEmailConfirmationResponse(await _accountsService.InitiateEmailConfirmationAsync(creatureId)));
    }

    [HttpPost]
    [Route("api/Users/{creatureId}/Email/Confirm")]
    public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmailAsync(Guid creatureId, ConfirmEmailRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }
        
        await CheckPrivilegesAsync(creatureId);

        return Ok(new ConfirmEmailResponse(await _accountsService.ConfirmEmailAsync(creatureId, request.Token)));
    }

    /// <summary>
    /// Initiate email change process
    /// </summary>
    [HttpPost]
    [Route("api/Users/{creatureId}/Email/InitiateChange")]
    public async Task<ActionResult<InitiateEmailChangeResponse>> InitiateEmailChangeAsync(Guid creatureId, InitiateEmailChangeRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }
        
        await CheckPrivilegesAsync(creatureId);

        var result = await _accountsService.InitiateEmailChangeAsync(creatureId, request.NewEmail);
        return Ok(new InitiateEmailChangeResponse(result.Item1, result.Item2));
    }
    
    /// <summary>
    /// Checks who can access profile data. If current user have no privileges to edit creatureId's profile - throws an exception
    /// </summary>
    private async Task CheckPrivilegesAsync(Guid creatureId)
    {
        var loggedIdCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);
        
        // TODO: Add support for admins
        if (loggedIdCreature.Id != creatureId)
        {
            throw new InvalidOperationException($"User with ID = { loggedIdCreature.Id } have no privileges to read/change user's with ID = { creatureId } data!");
        }
    }
}