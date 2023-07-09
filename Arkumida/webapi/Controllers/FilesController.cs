using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    /// Get categories
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
}