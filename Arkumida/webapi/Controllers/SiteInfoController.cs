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
        return Ok(new VersionInfoResponse("Аркумида-Д мод. 6", _siteInfoSettings.SourcesUrl));
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