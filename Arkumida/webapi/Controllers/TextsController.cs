using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Dao.Models.Enums;
using webapi.Models.Api.Requests;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with texts
/// </summary>
[Authorize]
[ApiController]
public class TextsController : ControllerBase
{
    private readonly ITextsService _textsService;
    
    public TextsController
    (
        ITextsService textsService
    )
    {
        _textsService = textsService;
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
                await _textsService.GetTextMetadataByIdAsync(id)
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
        return Ok
        (
            new TextPageResponse
            (
                await _textsService.GetTextPageAsync(id, pageNumber)
            )
        );
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

        var textsMetadata = await _textsService.GetTextsMetadataAsync(orderMode, skip, take);

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
        await _textsService.CreateTextAsync(textToCreate);

        return Ok
        (
            new CreateTextResponse(textToCreate.ToDto(_textsService))
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