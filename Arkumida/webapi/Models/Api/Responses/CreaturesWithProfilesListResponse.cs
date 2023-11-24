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
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response for methods, listing creatures (with profiles)
/// </summary>
public class CreaturesWithProfilesListResponse
{
    /// <summary>
    /// Listed creatures
    /// </summary>
    [JsonPropertyName("creatures")]
    public IReadOnlyCollection<CreatureWithProfileDto> Creatures { get; private set; }

    public CreaturesWithProfilesListResponse
    (
        IReadOnlyCollection<CreatureWithProfileDto> creatures
    )
    {
        Creatures = creatures ?? throw new ArgumentNullException(nameof(creatures), "Creatures list mustn't be null!");
    }
}