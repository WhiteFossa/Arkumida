using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Right menu controller
/// </summary>
[Authorize]
[ApiController]
public class RightMenuController : ControllerBase
{
    /// <summary>
    /// Get right menu items
    /// </summary>
    [AllowAnonymous]
    [Route("api/RightMenu/Items")]
    [HttpGet]
    public async Task<ActionResult<RightMenuResponse>> GetRightMenuItemsAsync()
    {
        var items = new List<ImagedLinkDto>()
        {
            new ImagedLinkDto("/messages", "", "Сообщения", "/images/message.png", "Сообщения", "inline-block vertical-align-center"),
            new ImagedLinkDto("/profile", "Первозвери", "Профиль", "/images/fossa_avatar.jpg", "Профиль", "right-menu-avatar")
        };
        
        return Ok(new RightMenuResponse(items));
    }
}