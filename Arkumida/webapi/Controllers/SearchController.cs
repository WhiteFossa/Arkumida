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

        return Ok(await _textsSearchService.SearchTextsAsync(request.Query, request.Skip, request.Take));
    }
}