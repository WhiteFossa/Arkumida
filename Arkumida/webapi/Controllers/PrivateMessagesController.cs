using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Requests.PrivateMessages;
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
    
    /// <summary>
    /// Get conversation with given creature
    /// </summary>
    [Route("api/PrivateMessages/Conversations/With/{creatureId}")]
    [HttpGet]
    public async Task<ActionResult<ConversationResponse>> GetConversationAsync(Guid creatureId)
    {
        var loggedInCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);
        
        var confidantProfile = await _accountsService.GetProfileByCreatureIdAsync(creatureId);

        var messages = await _privateMessagesService.GetConversationAsync(loggedInCreature.Id, creatureId);

        var lastMessageSentTime = messages
            .Select(m => m.SentTime)
            .OrderDescending()
            .FirstOrDefault();
        
        var messageDtos = messages
            .Select(m => m.ToDto())
            .ToList();
        
        return Ok(new ConversationResponse(confidantProfile.ToDto(), lastMessageSentTime, messageDtos));
    }

    /// <summary>
    /// Send private message
    /// </summary>
    [Route("api/PrivateMessages/SendTo/{receiverId}")]
    [HttpPost]
    public async Task<ActionResult<SentPrivateMessageResponse>> SendPrivateMessageAsync(Guid receiverId, SendPrivateMessageRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message must be non-empty!");
        }
        
        var loggedInCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);

        var result = await _privateMessagesService.SendPrivateMessageAsync(receiverId, loggedInCreature.Id, request.Message);

        return Ok(new SentPrivateMessageResponse(result.Item1, result.Item2));
    }

    [Route("api/PrivateMessages/MarkAsRead/{messageId}")]
    [HttpPost]
    public async Task<ActionResult<MarkPrivateMessageAsReadResponse>> MarkPrivateMessageAsRead(Guid messageId)
    {
        var loggedInCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);

        return Ok(new MarkPrivateMessageAsReadResponse(await _privateMessagesService.MarkPrivateMessageAsReadAsync(loggedInCreature.Id, messageId)));
    }
}