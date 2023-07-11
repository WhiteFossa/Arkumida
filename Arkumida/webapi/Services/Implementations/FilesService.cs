using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Helpers;
using webapi.Models.Api.DTOs;
using webapi.Services.Abstract;
using File = webapi.Models.File;

namespace webapi.Services.Implementations;

public class FilesService : IFilesService
{
    private readonly IFilesDao _filesDao;

    public FilesService
    (
        IFilesDao filesDao
    )
    {
        _filesDao = filesDao;
    }
    
    public async Task<FileInfoDto> UploadFileAsync(IFormFile file)
    {
        _ = file ?? throw new ArgumentNullException(nameof(file), "File must not be null!");

        var content = new byte[file.Length];
        using (var fileStream = file.OpenReadStream())
        {
            fileStream.Read(content, 0, (int)file.Length);
        }

        var fileDbo = new FileDbo()
        {
            Name = file.FileName,
            Type = file.ContentType,
            Content = content,
            Hash = SHA512Helper.CalculateSHA512(content)
        };

        await _filesDao.CreateFileAsync(fileDbo);

        return new FileInfoDto(fileDbo.Id, fileDbo.Name);
    }

    public async Task<File> GetFileAsync(Guid fileId)
    {
        var fileDbo = await _filesDao.GetFileAsync(fileId);

        return new File(fileDbo.Content, fileDbo.Type, fileDbo.Name, fileDbo.LastModifiedTime, fileDbo.Hash);
    }
}