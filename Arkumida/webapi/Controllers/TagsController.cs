using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Models.Api.Requests;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with tags
/// </summary>
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
    [Route("api/Tags/List")]
    [HttpGet]
    public async Task<ActionResult<TextTagsListResponse>> GetTagsAsync()
    {
        return Ok(new TextTagsListResponse
            (
                (await _tagsService.GetAllTagsAsync())
                .Select(t => t.ToTextTagDto())
                .ToList())
        );
    }
    
    /// <summary>
    /// Get tag by ID
    /// </summary>
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
    /// Create new tag
    /// </summary>
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