using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Constants;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller, working with basic site information
/// </summary>
[Authorize]
[ApiController]
public class SiteInfoController : ControllerBase
{
    private readonly IConfigurationService _configurationService;

    public SiteInfoController
    (
        IConfigurationService configurationService
    )
    {
        _configurationService = configurationService;
    }
    
    /// <summary>
    /// Get version info
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteInfo/Version")]
    [HttpGet]
    public async Task<ActionResult<VersionInfoResponse>> GetVersionInfoAsync()
    {
        return Ok(new VersionInfoResponse("Аркумида-В мод. 2", "https://github.com/WhiteFossa/Arkumida"));
    }
    
    /// <summary>
    /// Get site URL
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteInfo/Url")]
    [HttpGet]
    public async Task<ActionResult<SiteUrlResponse>> GetSiteUrlAsync()
    {
        var baseUrl = await _configurationService.GetConfigurationStringAsync(GlobalConstants.SiteInfoBaseUrlSettingName);
        var title = await _configurationService.GetConfigurationStringAsync(GlobalConstants.SiteInfoTitleSettingName);
        
        return Ok(new SiteUrlResponse(baseUrl, title));
    }
    
    /// <summary>
    /// Get telegram chat info
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteInfo/TelegramGroup")]
    [HttpGet]
    public async Task<ActionResult<TelegramGroupResponse>> GetTelegramGroupInfoAsync()
    {
        return Ok(new TelegramGroupResponse("https://t.me/joinchat/Fwu72wsdu6L-ufQKIi7JqQ", "Официальный чат Furtails"));
    }
}