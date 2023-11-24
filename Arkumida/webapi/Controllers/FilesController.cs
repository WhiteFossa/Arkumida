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
using Microsoft.Net.Http.Headers;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with files
/// </summary>
[Authorize]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFilesService _filesService;

    public FilesController
    (
        IFilesService filesService
    )
    {
        _filesService = filesService;
    }
    
    /// <summary>
    /// Upload a file
    /// </summary>
    [Route("api/Files/Upload")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<UploadFileResponse>> UploadAsync(IFormFile file)
    {
        try
        {
            return Ok(new UploadFileResponse(await _filesService.UploadFileAsync(file)));
        }
        catch (Exception)
        {
            return BadRequest("Error during file upload!");
        }
    }

    /// <summary>
    /// Download the file
    /// </summary>
    [AllowAnonymous]
    [Route("api/Files/Download/{fileId}")]
    [HttpGet]
    public async Task<ActionResult> DownloadAsync(Guid fileId)
    {
        try
        {
            var result = await _filesService.GetFileAsync(fileId);

            return File
                (
                    result.Content,
                    result.Type,
                    result.Name,
                    result.LastModifiedTime,
                    new EntityTagHeaderValue($"\"{ result.Hash }\"")
                );
        }
        catch (Exception)
        {
            return NotFound(); // Treating errors as not found too
        }
    }
}