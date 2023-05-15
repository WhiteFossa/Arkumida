using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Controller, returning information about current site version
/// </summary>
[ApiController]
public class VersionInfoController : ControllerBase
{
    /// <summary>
    /// Get version info
    /// </summary>
    [Route("api/VersionInfo/Get")]
    [HttpGet]
    public async Task<ActionResult<VersionInfoResponse>> GetVersionInfoAsync()
    {
        return Ok(new VersionInfoResponse("Arkumida-A mk.1"));
    }
}