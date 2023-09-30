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
    public async Task<ActionResult<PrivateMessagesCollectionResponse>> GetConversationAsync(Guid creatureId)
    {
        var loggedInCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);

        var messages = (await _privateMessagesService.GetConversationAsync(loggedInCreature.Id, creatureId))
            .Select(m => m.ToDto())
            .ToList();
        
        return Ok(new PrivateMessagesCollectionResponse(messages));
    }

    /// <summary>
    /// Get no more than limit of messages sent after startTime in conversation with creature, identified by creatureId.
    /// Messages are ordered by sentTime, from earlier to later
    /// </summary>
    [Route("api/PrivateMessages/Conversations/With/{creatureId}/After/{startTime}/Limit/{limit}")]
    [HttpGet]
    public async Task<ActionResult<PrivateMessagesCollectionResponse>> GetMessagesAfterTimeAsync(Guid creatureId, DateTime startTime, int limit)
    {
        var loggedInCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);
        
        var messages = (await _privateMessagesService.GetConversationAfterTimeWithLimitAsync(loggedInCreature.Id, creatureId, startTime, limit))
            .Select(m => m.ToDto())
            .ToList();
        
        return Ok(new PrivateMessagesCollectionResponse(messages));
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

    /// <summary>
    /// Mark private message as read
    /// </summary>
    [Route("api/PrivateMessages/MarkAsRead/{messageId}")]
    [HttpPost]
    public async Task<ActionResult<MarkPrivateMessageAsReadResponse>> MarkPrivateMessageAsRead(Guid messageId)
    {
        var loggedInCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);

        var markResult = await _privateMessagesService.MarkPrivateMessageAsReadAsync(loggedInCreature.Id, messageId);
        return Ok(new MarkPrivateMessageAsReadResponse(markResult.Item1, markResult.Item2));
    }

    /// <summary>
    /// Get all conversations summaries for current creature
    /// </summary>
    [Route("api/PrivateMessages/Conversations")]
    [HttpGet]
    public async Task<ActionResult<ConversationsSummariesResponse>> GetConversationsSummariesAsync()
    {
        var loggedInCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);
        
        return Ok(new ConversationsSummariesResponse(await _privateMessagesService.GetConversationsSummariesAsync(loggedInCreature.Id)));
    }
}