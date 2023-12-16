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
using Microsoft.Extensions.Options;
using webapi.Models.Api.Responses;
using webapi.Models.Settings;

namespace webapi.Controllers;

/// <summary>
/// Controller, working with basic site information
/// </summary>
[Authorize]
[ApiController]
public class SiteInfoController : ControllerBase
{
    private readonly SiteInfoSettings _siteInfoSettings;

    public SiteInfoController
    (
        IOptions<SiteInfoSettings> siteInfoSettings
    )
    {
        _siteInfoSettings = siteInfoSettings.Value;
    }
    
    /// <summary>
    /// Get version info
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteInfo/Version")]
    [HttpGet]
    public async Task<ActionResult<VersionInfoResponse>> GetVersionInfoAsync()
    {
        return Ok(new VersionInfoResponse("Аркумида-Д мод. 8", _siteInfoSettings.SourcesUrl));
    }
    
    /// <summary>
    /// Get site URL
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteInfo/Url")]
    [HttpGet]
    public async Task<ActionResult<SiteUrlResponse>> GetSiteUrlAsync()
    {
        return Ok(new SiteUrlResponse(_siteInfoSettings.BaseUrl, _siteInfoSettings.Title));
    }
    
    /// <summary>
    /// Get telegram chat info
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteInfo/TelegramGroup")]
    [HttpGet]
    public async Task<ActionResult<TelegramGroupResponse>> GetTelegramGroupInfoAsync()
    {
        return Ok(new TelegramGroupResponse(_siteInfoSettings.TelegramChatUrl, _siteInfoSettings.TelegramChatName));
    }
    
    /// <summary>
    /// Get site URL
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteInfo/Admin")]
    [HttpGet]
    public async Task<ActionResult<AdminInfoResponse>> GetSiteAdminInfoAsync()
    {
        return Ok(new AdminInfoResponse(_siteInfoSettings.AdminEmail));
    }
}