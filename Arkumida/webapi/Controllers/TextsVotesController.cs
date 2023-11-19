using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Helpers;
using webapi.Models.Api.Responses.TextsStatistics;
using webapi.Services.Abstract;
using webapi.Services.Abstract.TextsStatistics;

namespace webapi.Controllers;

/// <summary>
/// Controller to vote/get votes for texts
/// </summary>
[Authorize]
[ApiController]
public class TextsVotesController : ControllerBase
{
    private readonly ITextsStatisticsService _textsStatisticsService;
    private readonly IAccountsService _accountsService;

    public TextsVotesController
    (
        ITextsStatisticsService textsStatisticsService,
        IAccountsService accountsService
    )
    {
        _textsStatisticsService = textsStatisticsService;
        _accountsService = accountsService;
    }

    /// <summary>
    /// Is text liked?
    /// </summary>
    [Route("api/TextsVotes/IsLiked/{textId}")]
    [HttpGet]
    public async Task<ActionResult<IsTextLikedResponse>> IsTextLikedAsync(Guid textId)
    {
        var creatureId = (await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id;

        return Ok(new IsTextLikedResponse(await _textsStatisticsService.IsTextLikedAsync(textId, creatureId)));
    }
    
    /// <summary>
    /// Is text disliked?
    /// </summary>
    [Route("api/TextsVotes/IsDisliked/{textId}")]
    [HttpGet]
    public async Task<ActionResult<IsTextDislikedResponse>> IsTextDislikedAsync(Guid textId)
    {
        var creatureId = (await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id;

        return Ok(new IsTextDislikedResponse(await _textsStatisticsService.IsTextDislikedAsync(textId, creatureId)));
    }

    /// <summary>
    /// Like text
    /// </summary>
    [Route("api/TextsVotes/Like/{textId}")]
    [HttpPost]
    public async Task<ActionResult> LikeTextAsync(Guid textId)
    {
        var creatureId = (await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id;

        await _textsStatisticsService.LikeTextAsync
        (
            textId,
            creatureId,
            HttpContext.Connection.RemoteIpAddress.ToString(),
            UserAgentHelper.GetUserAgent(HttpContext)
        );
        
        return Ok();
    }
}