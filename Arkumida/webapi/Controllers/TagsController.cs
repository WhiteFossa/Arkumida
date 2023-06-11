using Microsoft.AspNetCore.Mvc;
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
                (await _tagsService.GetCategoriesTagsAsync())
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
        return Ok
        (
            new TextTagResponse((await _tagsService.GetTagByIdAsync(id)).ToTextTagDto())
        );
    }
}