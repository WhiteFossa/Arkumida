using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;
using webapi.Services.Abstract.TextsStatistics;

namespace webapi.Controllers;

/// <summary>
/// Controller, working with various statistics
/// </summary>
[Authorize]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly ITextsService _textsService;
    private readonly ITextsStatisticsService _textsStatisticsService;

    public StatisticsController
    (
        ITextsService textsService,
        ITextsStatisticsService textsStatisticsService
    )
    {
        _textsService = textsService;
        _textsStatisticsService = textsStatisticsService;
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
        var readsCount = await _textsStatisticsService.GetAllTextsCompleteReadsCountForLast24HoursAsync();
        var lastAddTime = await _textsService.GetLastTextAddTimeAsync();
        
        return Ok(new TextsStatisticsResponse(textsCount, readsCount, lastAddTime));
    }
}