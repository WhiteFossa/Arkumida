using webapi.Models.Api.DTOs;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with files
/// </summary>
public interface IFilesService
{
    /// <summary>
    /// Upload file from form to database
    /// </summary>
    Task<FileInfoDto> UploadFileAsync(IFormFile file);
}