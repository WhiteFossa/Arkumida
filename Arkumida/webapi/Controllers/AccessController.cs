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
using webapi.Models.Api.Responses.Access;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Access;

namespace webapi.Controllers;

/// <summary>
/// Controller, returning information about access to different endpoints
/// </summary>
[Authorize]
[ApiController]
public class AccessController : ControllerBase
{
    private readonly IAccountsService _accountsService;
    private readonly ITextsAccessService _textsAccessService;

    public AccessController
    (
        IAccountsService accountsService,
        ITextsAccessService textsAccessService
    )
    {
        _accountsService = accountsService;
        _textsAccessService = textsAccessService;
    }
    
    /// <summary>
    /// Is votes history visible?
    /// </summary>
    [AllowAnonymous]
    [Route("api/Access/Texts/{textId}/Votes/IsHistoryVisible")]
    [HttpGet]
    public async Task<ActionResult<AccessResponse>> IsTextVotesHistoryVisibleAsync(Guid textId)
    {
        var creatureId = User.Identity.IsAuthenticated ? (Guid?)(await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id : null;
        
        return Ok(new AccessResponse(await _textsAccessService.IsVotesHistoryVisibleAsync(textId, creatureId)));
    }
    
    /// <summary>
    /// Can creature vote for text?
    /// </summary>
    [AllowAnonymous]
    [Route("api/Access/Texts/{textId}/Votes/IsCanVote")]
    [HttpGet]
    public async Task<ActionResult<AccessResponse>> IsCanVoteForTextAsync(Guid textId)
    {
        var creatureId = User.Identity.IsAuthenticated ? (Guid?)(await _accountsService.FindUserByLoginAsync(User.Identity.Name)).Id : null;
        
        return Ok(new AccessResponse(await _textsAccessService.IsCanVoteForTextAsync(textId, creatureId)));
    }
}