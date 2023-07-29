using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Dao.Models.Enums;
using webapi.Models;
using webapi.Models.Api.Requests;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with tags
/// </summary>
[Authorize]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagsService _tagsService;

    public TagsController
    (
        ITagsService tagsService
    )
    {
        _tagsService = tagsService;
    }
    
    /// <summary>
    /// Get all tags
    /// </summary>
    [AllowAnonymous]
    [Route("api/Tags/List")]
    [HttpGet]
    public async Task<ActionResult<TextTagsListResponse>> GetTagsAsync([FromQuery]TagSubtype? subtype = null)
    {
        return Ok(new TextTagsListResponse
            (
                (await _tagsService.GetAllTagsAsync(subtype))
                .Select(t => t.ToTextTagDto())
                .ToList())
        );
    }
    
    /// <summary>
    /// Get tag by ID
    /// </summary>
    [AllowAnonymous]
    [Route("api/Tags/{id}")]
    [HttpGet]
    public async Task<ActionResult<TextTagResponse>> GetTagByIdAsync(Guid id)
    {
        Tag tag = null;

        try
        {
            tag = await _tagsService.GetTagByIdAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound("Tag with given ID not found.");
        }
        
        return Ok
        (
            new TextTagResponse(tag.ToTextTagDto())
        );
    }
    
    /// <summary>
    /// Get tag by name
    /// </summary>
    [AllowAnonymous]
    [Route("api/Tags/ByName")]
    [HttpGet]
    public async Task<ActionResult<TextTagResponse>> GetTagByNameAsync([FromQuery] string name)
    {
        Tag tag = null;

        try
        {
            tag = await _tagsService.GetTagByNameAsync(name);
        }
        catch (InvalidOperationException)
        {
            return NotFound("Tag with given name not found.");
        }
        
        return Ok
        (
            new TextTagResponse(tag.ToTextTagDto())
        );
    }

    /// <summary>
    /// Create new tag
    /// </summary>
    [AllowAnonymous] // TODO: Remove me
    [Route("api/Tags/Create")]
    [HttpPost]
    public async Task<ActionResult<CreateTagResponse>> CreateTagAsync([FromBody] CreateTagRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request must be provided.");
        }

        if (request.Tag == null)
        {
            return BadRequest("Tag must not be null");
        }

        var createdTag = request.Tag.ToTag();
        await _tagsService.CreateTagAsync(createdTag);

        return Ok
        (
            new CreateTagResponse(createdTag.ToTagDto())
        );
    }
}