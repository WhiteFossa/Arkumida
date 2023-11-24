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
/// Avatar
/// </summary>
public class AvatarDto
{
    /// <summary>
    /// ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Upload time (for ordering)
    /// </summary>
    [JsonPropertyName("uploadTime")]
    public DateTime UploadTime { get; set; }

    /// <summary>
    /// ID of avatar file
    /// </summary>
    [JsonPropertyName("fileId")]
    public Guid FileId { get; set; }

    /// <summary>
    /// Converts to avatar model (of course file is not loaded, only ID is provided)
    /// </summary>
    public Avatar ToModel()
    {
        return new Avatar()
        {
            Id = Id,
            Name = Name,
            UploadTime = UploadTime,
            File = new File() { Id = FileId}
        };
    }
}