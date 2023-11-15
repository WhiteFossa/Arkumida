using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Constants;
using webapi.Models.Api.Requests;
using webapi.Models.Api.Requests.TextsStatistics;
using webapi.Models.Api.Responses;
using webapi.Models.Api.Responses.TextsStatistics;
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

    /// <summary>
    /// Import statistics event
    /// </summary>
    [Authorize(Roles = RolesConstants.ImporterRole)]
    [Route("api/Statistics/ImportEvent")]
    [HttpPost]
    public async Task<ActionResult<ImportTextsStatisticsEventResponse>> ImportEventAsync([FromBody] ImportTextsStatisticsEventRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request must be provided.");
        }

        if (request.TextsStatisticsEvent == null)
        {
            return BadRequest("Event must not be null.");
        }

        var createdEvent = await _textsStatisticsService.AddTextStatisticsEventAsync
        (
            request.TextsStatisticsEvent.Type,
            request.TextsStatisticsEvent.Timestamp,
            request.TextsStatisticsEvent.TextId,
            request.TextsStatisticsEvent.Page,
            request.TextsStatisticsEvent.CreatureId,
            request.TextsStatisticsEvent.Ip,
            request.TextsStatisticsEvent.UserAgent
        );

        return Ok(new ImportTextsStatisticsEventResponse() { TextsStatisticsEvent = createdEvent.ToDto() });
    }
}