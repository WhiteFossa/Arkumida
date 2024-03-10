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
using webapi.Constants;
using webapi.Dao.Models.Enums;
using webapi.Dao.Models.Enums.Statistics;
using webapi.Helpers;
using webapi.Models.Api.Requests;
using webapi.Models.Api.Requests.TextsComments;
using webapi.Models.Api.Responses;
using webapi.Models.Api.Responses.TextsComments;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Forum;
using webapi.Services.Abstract.TextsStatistics;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with texts
/// </summary>
[Authorize]
[ApiController]
public class TextsController : ControllerBase
{
    private readonly ITextsService _textsService;
    private readonly ITextUtilsService _textUtilsService;
    private readonly ITextsStatisticsService _textsStatisticsService;
    private readonly IAccountsService _accountsService;
    private readonly IForumService _forumService;
    
    public TextsController
    (
        ITextsService textsService,
        ITextUtilsService textUtilsService,
        ITextsStatisticsService textsStatisticsService,
        IAccountsService accountsService,
        IForumService forumService
    )
    {
        _textsService = textsService;
        _textUtilsService = textUtilsService;
        _textsStatisticsService = textsStatisticsService;
        _accountsService = accountsService;
        _forumService = forumService;
    }

    /// <summary>
    /// Get text info by ID
    /// </summary>
    [AllowAnonymous]
    [Route("api/Texts/GetInfo/{textId}")]
    [HttpGet]
    public async Task<ActionResult<TextInfoResponse>> GetTextInfoAsync(Guid textId)
    {
        return Ok
        (
            new TextInfoResponse
            (
                await _textsService.GetTextInfoByIdAsync(textId)
            )
        );
    }
    
    /// <summary>
    /// Get text by ID
    /// </summary>
    [AllowAnonymous]
    [Route("api/Texts/GetReadData/{textId}")]
    [HttpGet]
    public async Task<ActionResult<TextReadResponse>> GetTextAsync(Guid textId)
    {
        return Ok
        (
            new TextReadResponse
            (
                await _textsService.GetTextToReadAsync(textId)
            )
        );
    }
    
    /// <summary>
    /// Get text page
    /// </summary>
    [AllowAnonymous]
    [Route("api/Texts/GetPage/{textId}/Page/{pageNumber}")]
    [HttpGet]
    public async Task<ActionResult<TextPageResponse>> GetTextPageAsync(Guid textId, int pageNumber)
    {
        var pageData = await _textsService.GetTextPageAsync(textId, pageNumber); 
        
        #region Page read event
        
        // If page data returned successfully we are going to add "page read" event
        var readerCreatureId = User.Identity.IsAuthenticated ? (Guid?)(await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id : null;
            
        await _textsStatisticsService.AddTextStatisticsEventAsync
        (
            TextsStatisticsEventType.PageRead,
            DateTime.UtcNow,
            textId,
            pageNumber,
            readerCreatureId,
            HttpContext.Connection.RemoteIpAddress.ToString(),
            UserAgentHelper.GetUserAgent(HttpContext)
        );
        
        #endregion
        
        #region Text completely read
        // If it was a last page of a text - we additionally generate a "text read completed" event
        var pagesCount = await _textsService.GetTextPagesCountAsync(textId);

        if (pageNumber == pagesCount)
        {
            await _textsStatisticsService.AddTextStatisticsEventAsync
            (
                TextsStatisticsEventType.TextReadCompleted,
                DateTime.UtcNow, 
                textId,
                pageNumber,
                readerCreatureId,
                HttpContext.Connection.RemoteIpAddress.ToString(),
                UserAgentHelper.GetUserAgent(HttpContext)
            );
        }
        
        #endregion
        
        return Ok(new TextPageResponse(pageData));
    }

    /// <summary>
    /// Get latest texts
    /// </summary>
    [AllowAnonymous]
    [Route("api/Texts/Latest")]
    [HttpGet]
    public async Task<ActionResult<TextsInfosListResponse>> GetLatestTextsAsync(int skip, int take)
    {
        return await GetTextsAsync(TextOrderMode.Latest, skip, take);
    }
    
    /// <summary>
    /// Get most popular texts
    /// </summary>
    [AllowAnonymous]
    [Route("api/Texts/Popular")]
    [HttpGet]
    public async Task<ActionResult<TextsInfosListResponse>> GetPopularTextsAsync(int skip, int take)
    {
        return await GetTextsAsync(TextOrderMode.Popular, skip, take);
    }

    private async Task<ActionResult<TextsInfosListResponse>> GetTextsAsync(TextOrderMode orderMode, int skip, int take)
    {
        if (skip < 0)
        {
            return BadRequest("Skip must be non-negative.");
        }

        if (take <= 0)
        {
            return BadRequest("Take must be positive.");
        }

        var textsCount = await _textsService.GetTotalTextsCountAsync();

        if (skip > textsCount)
        {
            return BadRequest("Skip too big.");
        }

        var textsMetadata = await _textsService.GetTextsInfosAsync(orderMode, skip, take);

        var remaining = Math.Max(0, textsCount - (skip + take));
        
        return Ok(new TextsInfosListResponse(textsMetadata, remaining));
    }
    
    /// <summary>
    /// Create new text
    /// </summary>
    [Route("api/Texts/Create")]
    [HttpPost]
    public async Task<ActionResult<CreateTextResponse>> CreateTextAsync([FromBody] CreateTextRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request must be provided.");
        }

        if (request.Text == null)
        {
            return BadRequest("Text must not be null.");
        }

        var textToCreate = request.Text.ToText();
        var createdText = await _textsService.CreateTextAsync(textToCreate);

        return Ok
        (
            new CreateTextResponse(createdText.ToDto(_textUtilsService))
        );
    }

    /// <summary>
    /// Add existing file to text
    /// </summary>
    [Route("api/Texts/AddFile")]
    [HttpPost]
    public async Task<ActionResult> AddFileToTextAsync([FromBody] AddFileToTextRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request must be provided.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Filename must not be empty.");
        }

        await _textsService.AddFileToTextAsync(request.TextId, request.Name, request.FileId);

        return Ok();
    }

    /// <summary>
    /// Import text comment
    /// </summary>
    [Authorize(Roles = RolesConstants.ImporterRole)]
    [Route("api/Texts/{textId}/ImportComment")]
    [HttpPost]
    public async Task<ActionResult<TextCommentAddedResponse>> ImportCommentAsync(Guid textId, [FromBody] ImportTextCommentRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request must be provided.");
        }

        return Ok(new TextCommentAddedResponse((await _forumService.AddTextCommentAsync(textId, request.Comment.ToForumMessage())).ToDto(_textUtilsService)));
    }

    
    /// <summary>
    /// Get text comments topic
    /// </summary>
    [AllowAnonymous]
    [Route("api/Texts/{textId}/GetCommentsTopic")]
    [HttpGet]
    public async Task<ActionResult<GetTextCommentsTopicResponse>> GetTextCommentsTopicAsync(Guid textId)
    {
        var commentsTopic = await _forumService.GetTextCommentsTopicByTextIdAsync(textId);

        return Ok(new GetTextCommentsTopicResponse(commentsTopic?.Id));
    }
}