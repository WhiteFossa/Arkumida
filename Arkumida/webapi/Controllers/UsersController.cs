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
    public async Task<ActionResult<UserRegistrationResponse>> CreateTextAsync([FromBody] UserRegistrationRequest request)
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
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)  
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
}