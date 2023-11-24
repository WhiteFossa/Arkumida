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

namespace webapi.Models;

/// <summary>
/// File (as for download)
/// </summary>
public class File
{
    /// <summary>
    /// File ID
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// File content
    /// </summary>
    public byte[] Content { get; set; }

    /// <summary>
    /// Furry-readable name (with extension)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// File type
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Last modified date
    /// </summary>
    public DateTime LastModifiedTime { get; set; }

    /// <summary>
    /// SHA-512 hash of file, to use as ETag
    /// </summary>
    public string Hash { get; set; }
}