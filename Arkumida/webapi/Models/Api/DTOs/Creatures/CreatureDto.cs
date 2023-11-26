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
using webapi.Models.Creatures;

namespace webapi.Models.Api.DTOs.Creatures;

/// <summary>
/// User
/// </summary>
public class CreatureDto : IdedEntityDto
{
    /// <summary>
    /// Creature login
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; private set; }

    /// <summary>
    /// Creature email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; private set; }

    public CreatureDto
    (
        Guid id,
        string furryReadableId,
        string login,
        string email
    ) : base(id, furryReadableId)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Login must be populated!", nameof(login));
        }
        Login = login;

        Email = email ?? throw new ArgumentNullException(nameof(email), "Email must not be null, at least empty string required!");
    }

    /// <summary>
    /// Build an user based on creature DTO. Please note that not all fields can be filled
    /// </summary>
    public Creature ToUser()
    {
        return new Creature(Id, Login, Email);
    }
}