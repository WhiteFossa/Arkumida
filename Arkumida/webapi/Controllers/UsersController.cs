using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Requests;
using webapi.Models.Api.Responses;
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

    public UsersController
    (
        IAccountsService accountsService
    )
    {
        _accountsService = accountsService;
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

        var registrationResult = await _accountsService.RegisterUserAsync(request.RegistrationData);

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
    [Route("api/Users/Current/CreateAvatar")]
    public async Task<ActionResult<CreateAvatarResponse>> CreateAvatarAsync([FromBody] CreateAvatarRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (request.Avatar == null)
        {
            return BadRequest("Avatar must be specified.");
        }

        var creature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);

        var createdAvatar = await _accountsService.AddAvatarAsync(creature.Id, request.Avatar.ToModel());

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

        await _accountsService.RenameAvatarAsync(creatureId, request.AvatarId, request.NewName);
        
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
}