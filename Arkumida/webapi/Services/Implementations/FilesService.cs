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

using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Helpers;
using webapi.Mappers.Abstract;
using webapi.Models.Api.DTOs;
using webapi.Services.Abstract;
using File = webapi.Models.File;

namespace webapi.Services.Implementations;

public class FilesService : IFilesService
{
    private readonly IFilesDao _filesDao;
    private readonly IFilesMapper _filesMapper;

    public FilesService
    (
        IFilesDao filesDao,
        IFilesMapper filesMapper
    )
    {
        _filesDao = filesDao;
        _filesMapper = filesMapper;
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
        return _filesMapper.Map(await _filesDao.GetFileAsync(fileId));
    }
}