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

using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

/// <summary>
/// File, stored in database
/// </summary>
public class FileDbo
{
    /// <summary>
    /// File ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Original file name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// MIME type
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// File content
    /// </summary>
    public byte[] Content { get; set; }
    
    /// <summary>
    /// SHA-512 of file content, for use as ETag
    /// </summary>
    public string Hash { get; set; }

    /// <summary>
    /// Last modification time
    /// </summary>
    public DateTime LastModifiedTime { get; set; }
}