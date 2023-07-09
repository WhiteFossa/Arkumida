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
    public async Task<ActionResult<UploadFileResponse>> UploadAsync(IFormFile file)
    {
        try
        {
            return Ok(await _filesService.UploadFileAsync(file));
        }
        catch (Exception)
        {
            return BadRequest("Error during file upload!");
        }
    }

    /// <summary>
    /// Download the file
    /// </summary>
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
                    result.LastModified,
                    new EntityTagHeaderValue($"\"{ result.Hash }\"")
                );
        }
        catch (Exception e)
        {
            return NotFound(); // Treating errors as not found too
        }
    }
}