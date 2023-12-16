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

using webapi.Models.Api.DTOs;
using webapi.Models.Api.DTOs.Creatures;

namespace webapi.Models.Creatures;

/// <summary>
/// Creature
/// </summary>
public class Creature
{
    /// <summary>
    /// User ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; }
    
    public Creature
    (
        Guid id,
        string login,
        string email
    )
    {
        // All fields except Id might be null when data is coming from the frontend
        Id = id;

        Login = login;
        Email = email;
    }
    
    /// <summary>
    /// Convert to CreatureDto
    /// </summary>
    public CreatureDto ToDto()
    {
        return new CreatureDto(Id, "not_ready", Login, Email);
    }
}