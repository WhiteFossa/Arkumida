using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Requests.Search;
using webapi.Models.Api.Responses.Search;
using webapi.Services.Abstract.Search;

namespace webapi.Controllers;

/// <summary>
/// Controller to search texts
/// </summary>
[Authorize]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ITextsSearchService _textsSearchService;

    public SearchController
    (
        ITextsSearchService textsSearchService
    )
    {
        _textsSearchService = textsSearchService;
    }

    /// <summary>
    /// Search texts
    /// </summary>
    [AllowAnonymous]
    [Route("api/Search/Texts")]
    [HttpPost]
    public async Task<ActionResult<TextsSearchResultsResponse>> SearchTextsAsync(TextsSearchRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        return Ok(await _textsSearchService.SearchTextsAsync(request.Query));
    }
}