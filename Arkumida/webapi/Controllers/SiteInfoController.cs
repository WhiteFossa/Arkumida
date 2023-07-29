using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Controller, working with basic site information
/// </summary>
[Authorize]
[ApiController]
public class SiteInfoController : ControllerBase
{
    /// <summary>
    /// Get version info
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteInfo/Version")]
    [HttpGet]
    public async Task<ActionResult<VersionInfoResponse>> GetVersionInfoAsync()
    {
        return Ok(new VersionInfoResponse("Аркумида-Б мод. 1", "https://github.com/WhiteFossa/Arkumida"));
    }
    
    /// <summary>
    /// Get site URL
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteInfo/Url")]
    [HttpGet]
    public async Task<ActionResult<SiteUrlResponse>> GetSiteUrlAsync()
    {
        return Ok(new SiteUrlResponse("https://arkumida.furtails.pw", "arkumida.furtails.pw"));
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