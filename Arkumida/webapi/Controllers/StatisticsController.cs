using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller, working with various statistics
/// </summary>
[Authorize]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly ITextsService _textsService;

    public StatisticsController(ITextsService textsService)
    {
        _textsService = textsService;
    }
    
    /// <summary>
    /// Get texts statistics
    /// </summary>
    [AllowAnonymous]
    [Route("api/Statistics/Texts")]
    [HttpGet]
    public async Task<ActionResult<TextsStatisticsResponse>> GetTextsStatisticsAsync()
    {
        var textsCount = await _textsService.GetTotalTextsCountAsync();
        var lastAddTime = await _textsService.GetLastTextAddTimeAsync();
        
        return Ok(new TextsStatisticsResponse(textsCount, 1337, lastAddTime));
    }
}