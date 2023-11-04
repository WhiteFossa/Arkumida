using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Dao.Models.Enums;
using webapi.Dao.Models.Enums.Statistics;
using webapi.Helpers;
using webapi.Models.Api.Requests;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;
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
    
    public TextsController
    (
        ITextsService textsService,
        ITextUtilsService textUtilsService,
        ITextsStatisticsService textsStatisticsService,
        IAccountsService accountsService
    )
    {
        _textsService = textsService;
        _textUtilsService = textUtilsService;
        _textsStatisticsService = textsStatisticsService;
        _accountsService = accountsService;
    }

    /// <summary>
    /// Get text info by ID
    /// </summary>
    [AllowAnonymous]
    [Route("api/Texts/GetInfo/{id}")]
    [HttpGet]
    public async Task<ActionResult<TextInfoResponse>> GetTextInfoAsync(Guid id)
    {
        return Ok
        (
            new TextInfoResponse
            (
                await _textsService.GetTextInfoByIdAsync(id)
            )
        );
    }
    
    /// <summary>
    /// Get text by ID
    /// </summary>
    [AllowAnonymous]
    [Route("api/Texts/GetReadData/{id}")]
    [HttpGet]
    public async Task<ActionResult<TextReadResponse>> GetTextAsync(Guid id)
    {
        return Ok
        (
            new TextReadResponse
            (
                await _textsService.GetTextToReadAsync(id)
            )
        );
    }
    
    /// <summary>
    /// Get text page
    /// </summary>
    [AllowAnonymous]
    [Route("api/Texts/GetPage/{id}/Page/{pageNumber}")]
    [HttpGet]
    public async Task<ActionResult<TextPageResponse>> GetTextPageAsync(Guid id, int pageNumber)
    {
        var pageData = await _textsService.GetTextPageAsync(id, pageNumber); 
        
        // If page data returned successfully we are going to add "read" event
        var readerCreatureId = User.Identity.IsAuthenticated ? (Guid?)(await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id : null;
            
        await _textsStatisticsService.AddTextStatisticsEventAsync
        (
            TextsStatisticsEventType.Read,
            id,
            pageNumber,
            readerCreatureId,
            HttpContext.Connection.RemoteIpAddress.ToString(),
            UserAgentHelper.GetUserAgent(HttpContext)
        );
        
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
}