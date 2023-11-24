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
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Main menu controller
/// </summary>
[Authorize]
[ApiController]
public class MainMenuController : ControllerBase
{
    /// <summary>
    /// Get main menu items
    /// </summary>
    [AllowAnonymous]
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