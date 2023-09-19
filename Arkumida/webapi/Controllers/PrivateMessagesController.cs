using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Responses.PrivateMessages;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with private messages
/// </summary>
[Authorize]
[ApiController]
public class PrivateMessagesController : ControllerBase
{
    private readonly IAccountsService _accountsService;
    private readonly IPrivateMessagesService _privateMessagesService;

    public PrivateMessagesController
    (
        IAccountsService accountsService,
        IPrivateMessagesService privateMessagesService
    )
    {
        _accountsService = accountsService;
        _privateMessagesService = privateMessagesService;
    }
    
    /// <summary>
    /// Get information about unread private messages
    /// </summary>
    [Route("api/PrivateMessages/UnreadInfo")]
    [HttpGet]
    public async Task<ActionResult<UnreadPrivateMessagesInfoResponse>> GetUnreadInfoAsync()
    {
        var loggedInCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);
        
        return Ok(new UnreadPrivateMessagesInfoResponse(await _privateMessagesService.GetUnreadPrivateMessagesCountAsync(loggedInCreature.Id)));
    }
}