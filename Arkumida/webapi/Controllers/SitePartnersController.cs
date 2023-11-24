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
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with site partners
/// </summary>
[Authorize]
[ApiController]
public class SitePartnersController : ControllerBase
{
    /// <summary>
    /// Get site partners
    /// </summary>
    [AllowAnonymous]
    [Route("api/SitePartners/Get")]
    [HttpGet]
    public async Task<ActionResult<SitePartnersResponse>> GetSitePartnersAsync()
    {
        var partners = new List<SitePartnerDto>()
        {
            new SitePartnerDto(new Guid("04e054da-03d3-4114-991f-7e2bb0f483eb"), "http://furry-world.ru/", "Furry World", "/images/banners/furry-world.ru.webp", "Furry World"),
            new SitePartnerDto(new Guid("89c6962b-c83b-4374-ae12-c21ca4e42e8f"), "http://foxcomix.ru/", "Новые стрипы каждый понедельник", "/images/banners/foxcomix.webp", "Лисий комикс"),
            new SitePartnerDto(new Guid("28d58366-788e-495b-96f3-dd537ff83025"), "http://sssradio.ru/", "Субкультурное радио", "/images/banners/sssradio.ru.webp", "Субкультурное радио"),
            new SitePartnerDto(new Guid("4813e160-784e-42cd-a532-517e115f9736"), "http://www.onegai.in/", "Фурри Йифф Хентай Юри Яой - Онегай!", "/images/banners/onegai.webp", "Фурри Йифф Хентай Юри Яой - Онегай!")
        };

        return Ok(new SitePartnersResponse(partners));
    }
}