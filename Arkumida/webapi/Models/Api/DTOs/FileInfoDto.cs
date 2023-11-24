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

using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// DTO with information on file
/// </summary>
public class FileInfoDto
{
    /// <summary>
    /// File ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; private set; }

    /// <summary>
    /// Filename
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; private set; }
    
    public FileInfoDto
    (
        Guid id,
        string name
    )
    {
        Id = id;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("File name must be populated!", nameof(name));
        }
        Name = name;
    }
}