using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Controller, working with various statistics
/// </summary>
[ApiController]
public class StatisticsController : ControllerBase
{
    /// <summary>
    /// Get texts statistics
    /// </summary>
    [Route("api/Statistics/Texts")]
    [HttpGet]
    public async Task<ActionResult<TextsStatisticsResponse>> GetTextsStatisticsAsync()
    {
        return Ok(new TextsStatisticsResponse(3662, 1337, DateTime.UtcNow));
    }
}