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
using File = webapi.Models.File;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for files
/// </summary>
public interface IFilesMapper
{
    IReadOnlyCollection<File> Map(IEnumerable<FileDbo> files);

    File Map(FileDbo file);

    FileDbo Map(File file);

    IReadOnlyCollection<FileDbo> Map(IEnumerable<File> files);
}