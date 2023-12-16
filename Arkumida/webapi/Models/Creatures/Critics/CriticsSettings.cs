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

using webapi.Models.Api.DTOs.Creatures.Critics;

namespace webapi.Models.Creatures.Critics;

/// <summary>
/// Critics settings for creature
/// </summary>
public class CriticsSettings
{
    /// <summary>
    /// Is show dislikes to creature?
    /// </summary>
    public bool IsShowDislikes { get; set; }

    /// <summary>
    /// Is show dislikes authors?
    /// </summary>
    public bool IsShowDislikesAuthors { get; set; }

    /// <summary>
    /// Convert to DTO
    /// </summary>
    public CriticsSettingsDto ToDto()
    {
        return new CriticsSettingsDto()
        {
            IsShowDislikes = IsShowDislikes,
            IsShowDislikesAuthors = IsShowDislikesAuthors
        };
    }
}