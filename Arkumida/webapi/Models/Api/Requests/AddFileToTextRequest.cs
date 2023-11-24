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

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to add already uploaded files to texts
/// </summary>
public class AddFileToTextRequest
{
    /// <summary>
    /// Add file to this text
    /// </summary>
    [JsonPropertyName("textId")]
    public Guid TextId { get; set; }

    /// <summary>
    /// Add this file to text
    /// </summary>
    [JsonPropertyName("fileId")]
    public Guid FileId { get; set; }

    /// <summary>
    /// Add file under this name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}