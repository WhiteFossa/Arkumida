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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Helpers;
using webapi.Models.Api.Responses.TextsStatistics;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Access;
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
    private readonly ITextsAccessService _textsAccessService;

    public TextsVotesController
    (
        ITextsStatisticsService textsStatisticsService,
        IAccountsService accountsService,
        ITextsAccessService textsAccessService
    )
    {
        _textsStatisticsService = textsStatisticsService;
        _accountsService = accountsService;
        _textsAccessService = textsAccessService;
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
    public async Task<ActionResult<LikeResponse>> LikeTextAsync(Guid textId)
    {
        var creatureId = (await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id;

        var isSuccessful = await _textsStatisticsService.LikeTextAsync
        (
            textId,
            creatureId,
            HttpContext.Connection.RemoteIpAddress.ToString(),
            UserAgentHelper.GetUserAgent(HttpContext)
        );
        
        return Ok(new LikeResponse(isSuccessful));
    }
    
    /// <summary>
    /// Unlike text
    /// </summary>
    [Route("api/TextsVotes/Unlike/{textId}")]
    [HttpPost]
    public async Task<ActionResult<UnlikeResponse>> UnlikeTextAsync(Guid textId)
    {
        var creatureId = (await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id;

        var isSuccessful = await _textsStatisticsService.UnlikeTextAsync
        (
            textId,
            creatureId,
            HttpContext.Connection.RemoteIpAddress.ToString(),
            UserAgentHelper.GetUserAgent(HttpContext)
        );
        
        return Ok(new UnlikeResponse(isSuccessful));
    }
    
    /// <summary>
    /// Dislike text
    /// </summary>
    [Route("api/TextsVotes/Dislike/{textId}")]
    [HttpPost]
    public async Task<ActionResult<DislikeResponse>> DislikeTextAsync(Guid textId)
    {
        var creatureId = (await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id;

        var isSuccessful = await _textsStatisticsService.DislikeTextAsync
        (
            textId,
            creatureId,
            HttpContext.Connection.RemoteIpAddress.ToString(),
            UserAgentHelper.GetUserAgent(HttpContext)
        );
        
        return Ok(new DislikeResponse(isSuccessful));
    }
    
    /// <summary>
    /// Undislike text
    /// </summary>
    [Route("api/TextsVotes/Undislike/{textId}")]
    [HttpPost]
    public async Task<ActionResult<UndislikeResponse>> UndislikeTextAsync(Guid textId)
    {
        var creatureId = (await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id;

        var isSuccessful = await _textsStatisticsService.UndislikeTextAsync
        (
            textId,
            creatureId,
            HttpContext.Connection.RemoteIpAddress.ToString(),
            UserAgentHelper.GetUserAgent(HttpContext)
        );
        
        return Ok(new UndislikeResponse(isSuccessful));
    }
    
    /// <summary>
    /// Likes count. Likes are anonymous-accessible, dislikes - not
    /// </summary>
    [AllowAnonymous]
    [Route("api/TextsVotes/LikesCount/{textId}")]
    [HttpGet]
    public async Task<ActionResult<LikesCountResponse>> GetLikesCountAsync(Guid textId)
    {
        return Ok(new LikesCountResponse(await _textsStatisticsService.GetLikesCountAsync(textId)));
    }
    
    /// <summary>
    /// Dislikes count. Likes are anonymous-accessible, dislikes - not
    /// TODO: Add finely-grained access control
    /// </summary>
    [Route("api/TextsVotes/DislikesCount/{textId}")]
    [HttpGet]
    public async Task<ActionResult<LikesCountResponse>> GetDislikesCountAsync(Guid textId)
    {
        return Ok(new DislikesCountResponse(await _textsStatisticsService.GetDislikesCountAsync(textId)));
    }

    /// <summary>
    /// Get votes list for given text
    /// </summary>
    [Route("api/TextsVotes/VotesList/{textId}")]
    [HttpGet]
    public async Task<ActionResult<TextVotesResponse>> GetTextVotesAsync(Guid textId)
    {
        var creatureId = (await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id;

        if (!await _textsAccessService.IsVotesHistoryVisibleAsync(textId, creatureId))
        {
            return Unauthorized();
        }

        return Ok(new TextVotesResponse(await _textsStatisticsService.GetVotesEventsAsync(textId, creatureId)));
    }
}