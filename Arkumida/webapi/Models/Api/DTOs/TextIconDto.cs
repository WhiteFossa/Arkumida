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
using webapi.Models.Enums;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Text icon
/// </summary>
public class TextIconDto
{
    /// <summary>
    /// Icon type
    /// </summary>
    [JsonPropertyName("type")]
    public TextIconType Type { get; private set; }

    /// <summary>
    /// Optional URL (if set, then icon will be a link)
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; private set; }

    public TextIconDto
    (
        TextIconType type,
        string url = ""
    )
    {
        Type = type;
        Url = url;
    }
}