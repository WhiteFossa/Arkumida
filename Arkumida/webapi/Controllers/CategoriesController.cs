using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with categories
/// </summary>
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
    [Route("api/Categories/List")]
    [HttpGet]
    public async Task<ActionResult<CategoriesTagsListResponse>> GetCategories()
    {
        return Ok(new CategoriesTagsListResponse
        (
            (await _tagsService.GetCategoriesTagsAsync())
            .Select(t => t.ToCategoryTagDto())
            .ToList())
        );
    }
}