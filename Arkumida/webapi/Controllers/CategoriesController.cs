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
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with categories
/// </summary>
[Authorize]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ITagsService _tagsService;

    public CategoriesController(ITagsService tagsService)
    {
        _tagsService = tagsService;
    }

    /// <summary>
    /// Get categories
    /// </summary>
    [AllowAnonymous]
    [Route("api/Categories/List")]
    [HttpGet]
    public async Task<ActionResult<CategoriesTagsListResponse>> GetCategoriesAsync()
    {
        return Ok(new CategoriesTagsListResponse
        (
            (await _tagsService.GetCategoriesTagsAsync())
            .Select(t => t.ToCategoryTagDto())
            .ToList())
        );
    }

    /// <summary>
    /// Get category by ID
    /// </summary>
    [AllowAnonymous]
    [Route("api/Categories/{id}")]
    [HttpGet]
    public async Task<ActionResult<CategoryTagResponse>> GetCategoryByIdAsync(Guid id)
    {
        return Ok
        (
            new CategoryTagResponse((await _tagsService.GetTagByIdAsync(id)).ToCategoryTagDto())
        );
    }
}