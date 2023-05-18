using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Controller, working with basic site information
/// </summary>
[ApiController]
public class SiteInfoController : ControllerBase
{
    /// <summary>
    /// Get version info
    /// </summary>
    [Route("api/SiteInfo/Version")]
    [HttpGet]
    public async Task<ActionResult<VersionInfoResponse>> GetVersionInfoAsync()
    {
        return Ok(new VersionInfoResponse("Аркумида-А мод. 1"));
    }
    
    /// <summary>
    /// Get site URL
    /// </summary>
    [Route("api/SiteInfo/Url")]
    [HttpGet]
    public async Task<ActionResult<SiteUrlResponse>> GetSiteUrlAsync()
    {
        return Ok(new SiteUrlResponse("https://furtails.pw", "furtails.pw"));
    }
}