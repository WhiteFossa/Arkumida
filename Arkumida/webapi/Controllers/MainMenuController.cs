using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Main menu controller
/// </summary>
[ApiController]
public class MainMenuController : ControllerBase
{
    /// <summary>
    /// Get main menu items
    /// </summary>
    [Route("api/MainMenu/Items")]
    [HttpGet]
    public async Task<ActionResult<MainMenuResponse>> GetMainMenuItemsAsync()
    {
        var items = new List<LinkDto>()
        {
            new LinkDto("/we", "Мы", "Мы"),
            new LinkDto("/forum", "Форум", "Форум"),
            new LinkDto("/faq", "FAQ", "FAQ"),
            new LinkDto("/feedback", "Обратная связь", "Обратная связь"),
        };

        return Ok(new MainMenuResponse(items));
    }
}