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
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Controller, working with site-wide notifications
/// </summary>
[Authorize]
[ApiController]
public class SiteNotificationsController : ControllerBase
{
    /// <summary>
    /// Get donate to Redgerra info
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteNotifications/DonateToRedgerra")]
    [HttpGet]
    public async Task<ActionResult<DonateToRegderraInfoResponse>> GetDonateToRedgerraInfoAsync()
    {
        return Ok(new DonateToRegderraInfoResponse("https://next.privat24.ua/money-transfer/card", "5457 0822 7173 3102"));
    }
}