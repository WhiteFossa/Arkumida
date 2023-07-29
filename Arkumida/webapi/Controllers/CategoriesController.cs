using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with categories
/// </summary>
[Authorize]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ITagsService _tagsService;

    public CategoriesController(ITagsService tagsService)
    {
        _tagsService = tagsService;
    }

    /// <summary>
    /// Get categories
    /// </summary>
    [AllowAnonymous]
    [Route("api/Categories/List")]
    [HttpGet]
    public async Task<ActionResult<CategoriesTagsListResponse>> GetCategoriesAsync()
    {
        return Ok(new CategoriesTagsListResponse
        (
            (await _tagsService.GetCategoriesTagsAsync())
            .Select(t => t.ToCategoryTagDto())
            .ToList())
        );
    }

    /// <summary>
    /// Get category by ID
    /// </summary>
    [AllowAnonymous]
    [Route("api/Categories/{id}")]
    [HttpGet]
    public async Task<ActionResult<CategoryTagResponse>> GetCategoryByIdAsync(Guid id)
    {
        return Ok
        (
            new CategoryTagResponse((await _tagsService.GetTagByIdAsync(id)).ToCategoryTagDto())
        );
    }
}