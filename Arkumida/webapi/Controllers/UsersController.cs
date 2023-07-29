using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}