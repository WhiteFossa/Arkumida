using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Right menu controller
/// </summary>
[ApiController]
public class RightMenuController : ControllerBase
{
    /// <summary>
    /// Get right menu items
    /// </summary>
    [Route("api/RightMenu/Items")]
    [HttpGet]
    public async Task<ActionResult<RightMenuResponse>> GetRightMenuItemsAsync()
    {
        var items = new List<ImagedLinkDto>()
        {
            new ImagedLinkDto("/messages", "", "Сообщения", "/images/message.png", "Сообщения", "inline-block vertical-align-center"),
            new ImagedLinkDto("/profile", "Фосса", "Профиль", "/images/fossa_avatar.jpg", "Профиль", "right-menu-avatar")
        };
        
        return Ok(new RightMenuResponse(items));
    }
}