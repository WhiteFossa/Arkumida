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

using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using File = webapi.Models.File;

namespace webapi.Mappers.Implementations;

public class FilesMapper : IFilesMapper
{
    public IReadOnlyCollection<File> Map(IEnumerable<FileDbo> files)
    {
        if (files == null)
        {
            return null;
        }

        return files.Select(f => Map(f)).ToList();
    }

    public File Map(FileDbo file)
    {
        if (file == null)
        {
            return null;
        }

        return new File()
        {
            Id = file.Id,
            Content = file.Content,
            Hash = file.Hash,
            Name = file.Name,
            Type = file.Type,
            LastModifiedTime = file.LastModifiedTime
        };
    }

    public FileDbo Map(File file)
    {
        if (file == null)
        {
            return null;
        }

        return new FileDbo()
        {
            Id = file.Id,
            Content = file.Content,
            Hash = file.Hash,
            Name = file.Name,
            Type = file.Type,
            LastModifiedTime = file.LastModifiedTime
        };
    }

    public IReadOnlyCollection<FileDbo> Map(IEnumerable<File> files)
    {
        if (files == null)
        {
            return null;
        }

        return files.Select(f => Map(f)).ToList();
    }
}