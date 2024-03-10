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
using webapi.Models.Api.DTOs.Forum;
using webapi.Models.Api.Requests.Forum;
using webapi.Models.Api.Responses.Forum;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Forum;

namespace webapi.Controllers;

/// <summary>
/// Controller with forum-related endpoints
/// </summary>
[Authorize]
[ApiController]
public class ForumController : ControllerBase
{
    private readonly IForumService _forumService;
    private readonly ITextUtilsService _textUtilsService;
    private readonly IAccountsService _accountsService;

    public ForumController
    (
        IForumService forumService,
        ITextUtilsService textUtilsService,
        IAccountsService accountsService
    )
    {
        _forumService = forumService;
        _textUtilsService = textUtilsService;
        _accountsService = accountsService;
    }
    
    /// <summary>
    /// Get forum topic metadata
    /// </summary>
    [AllowAnonymous]
    [Route("api/Forum/Topics/{topicId}/GetInfo")]
    [HttpGet]
    public async Task<ActionResult<ForumTopicInfoResponse>> GetTopicInfoAsync(Guid topicId)
    {
        var topicInfo = await _forumService.GetTopicInfoAsync(topicId);
        if (topicInfo == null)
        {
            return NotFound();
        }
        
        return Ok(new ForumTopicInfoResponse(topicInfo.ToDto(_textUtilsService)));
    }

    /// <summary>
    /// Get forum topic messages
    /// </summary>
    [AllowAnonymous]
    [Route("api/Forum/Topics/{topicId}/Messages")]
    [HttpGet]
    public async Task<ActionResult<ForumTopicMessagesResponse>> GetTopicMessagesAsync(Guid topicId, int skip, int take)
    {
        var messages = await _forumService.GetLastMessagesInTopicAsync(topicId, skip, take);
        
        return Ok
        (
            new ForumTopicMessagesResponse
            (
                topicId,
                skip,
                take,
                messages.Select(m => m.ToDto(_textUtilsService)).ToList()
            )
        );
    }

    /// <summary>
    /// Add message to forum
    /// </summary>
    [Route("api/Forum/Topics/{topicId}/Messages")]
    [HttpPost]
    public async Task<ActionResult<ForumMessageAddedResponse>> AddMessageAsync(Guid topicId, [FromBody] AddForumMessageRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request must be provided.");
        }
        
        var loggedInCreature = await _accountsService.FindUserByLoginAsync(User.Identity.Name);

        return Ok
        (
            new ForumMessageAddedResponse
            (
                (await _forumService.AddMessageAsync
                    (
                        topicId,
                        loggedInCreature.Id,
                        request.Message.ReplyTo,
                        request.Message.Message
                    )
                ).ToDto(_textUtilsService)
            )
        );
    }
}