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
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class TextFilesMapper : ITextFilesMapper
{
    private readonly IFilesMapper _filesMapper;

    public TextFilesMapper(IFilesMapper filesMapper)
    {
        _filesMapper = filesMapper;
    }
    
    public IReadOnlyCollection<TextFile> Map(IEnumerable<TextFileDbo> textFiles)
    {
        if (textFiles == null)
        {
            return null;
        }

        return textFiles.Select(tf => Map(tf)).ToList();
    }

    public TextFile Map(TextFileDbo textFile)
    {
        return new TextFile()
        {
            Id = textFile.Id,
            Name = textFile.Name,
            File = _filesMapper.Map(textFile.File)
        };
    }

    public TextFileDbo Map(TextFile textFile)
    {
        return new TextFileDbo()
        {
            Id = textFile.Id,
            Name = textFile.Name,
            File = _filesMapper.Map(textFile.File)
        };
    }

    public IReadOnlyCollection<TextFileDbo> Map(IEnumerable<TextFile> textFiles)
    {
        if (textFiles == null)
        {
            return null;
        }

        return textFiles.Select(tf => Map(tf)).ToList();
    }
}