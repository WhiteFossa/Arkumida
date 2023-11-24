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

using webapi.Dao.Models;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with avatars
/// </summary>
public interface IAvatarsDao
{
    /// <summary>
    /// Creates avatar and adds it to creature's collection
    /// </summary>
    Task<AvatarDbo> AddAvatarToCreatureAsync(Guid creatureId, AvatarDbo avatar);

    /// <summary>
    /// Update avatar
    /// </summary>
    Task<AvatarDbo> UpdateAvatarAsync(AvatarDbo avatarToUpdate);

    /// <summary>
    /// Delete avatar
    /// </summary>
    Task DeleteAvatarAsync(Guid avatarId);

    /// <summary>
    /// Get avatar by Id (may return null in case of incorrect avatar)
    /// </summary>
    Task<AvatarDbo> GetAvatarByIdAsync(Guid avatarId);
}