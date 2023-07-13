using webapi.Models.Api.DTOs;
using File = webapi.Models.File;

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

    /// <summary>
    /// Get file (for download). If fileId is incorrect - throws an exception
    /// </summary>
    Task<File> GetFileAsync(Guid fileId);
}