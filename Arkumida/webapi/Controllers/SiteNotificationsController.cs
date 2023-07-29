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