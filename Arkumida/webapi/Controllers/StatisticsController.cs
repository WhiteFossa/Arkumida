#region License
// Arkumida - Furtails.pw next generation backend
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Constants;
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