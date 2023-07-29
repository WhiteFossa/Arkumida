using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using webapi.Models.Api.DTOs;
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
    [AllowAnonymous] // TODO: Remove me
    [Route("api/Files/Upload")]
    [HttpPost]
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