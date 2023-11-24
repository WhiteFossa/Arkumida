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
using webapi.Dao.Models.Enums;
using webapi.Models;
using webapi.Models.Api.Requests;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with tags
/// </summary>
[Authorize]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagsService _tagsService;

    public TagsController
    (
        ITagsService tagsService
    )
    {
        _tagsService = tagsService;
    }
    
    /// <summary>
    /// Get all tags
    /// </summary>
    [AllowAnonymous]
    [Route("api/Tags/List")]
    [HttpGet]
    public async Task<ActionResult<TextTagsListResponse>> GetTagsAsync([FromQuery]TagSubtype? subtype = null)
    {
        return Ok(new TextTagsListResponse
            (
                (await _tagsService.GetAllTagsAsync(subtype))
                .Select(t => t.ToTextTagDto())
                .ToList())
        );
    }
    
    /// <summary>
    /// Get tag by ID
    /// </summary>
    [AllowAnonymous]
    [Route("api/Tags/{id}")]
    [HttpGet]
    public async Task<ActionResult<TextTagResponse>> GetTagByIdAsync(Guid id)
    {
        Tag tag = null;

        try
        {
            tag = await _tagsService.GetTagByIdAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound("Tag with given ID not found.");
        }
        
        return Ok
        (
            new TextTagResponse(tag.ToTextTagDto())
        );
    }
    
    /// <summary>
    /// Get tag by name
    /// </summary>
    [AllowAnonymous]
    [Route("api/Tags/ByName")]
    [HttpGet]
    public async Task<ActionResult<TextTagResponse>> GetTagByNameAsync([FromQuery] string name)
    {
        Tag tag = null;

        try
        {
            tag = await _tagsService.GetTagByNameAsync(name);
        }
        catch (InvalidOperationException)
        {
            return NotFound("Tag with given name not found.");
        }
        
        return Ok
        (
            new TextTagResponse(tag.ToTextTagDto())
        );
    }

    /// <summary>
    /// Create new tag
    /// </summary>
    [Route("api/Tags/Create")]
    [HttpPost]
    public async Task<ActionResult<CreateTagResponse>> CreateTagAsync([FromBody] CreateTagRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request must be provided.");
        }

        if (request.Tag == null)
        {
            return BadRequest("Tag must not be null");
        }

        var createdTag = request.Tag.ToTag();
        await _tagsService.CreateTagAsync(createdTag);

        return Ok
        (
            new CreateTagResponse(createdTag.ToTagDto())
        );
    }
}