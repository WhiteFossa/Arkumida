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
/// Arkumida user profile
/// </summary>
public class CreatureProfileDbo
{
    /// <summary>
    /// Profile ID (must match CreatureDbo ID)
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// If true, then creature must change hir password
    /// </summary>
    public bool IsPasswordChangeRequired { get; set; }

    /// <summary>
    /// Plaintext password, it is used to send notifications to creatures about "You are registered on Arkumida now, your password is..." if case of migration from furtails.pw
    /// After password change it should be wiped-out
    /// </summary>
    public string OneTimePlaintextPassword { get; set; }

    /// <summary>
    /// User's visible name
    /// </summary>
    public string DisplayName { get; set; }
    
    /// <summary>
    /// Creature's avatars
    /// </summary>
    public IList<AvatarDbo> Avatars { get; set; }

    /// <summary>
    /// Creature's current avatar
    /// </summary>
    public AvatarDbo CurrentAvatar { get; set; }

    /// <summary>
    /// Information about creature (in the format, processable by ITextUtilsService.ParseTextToElements())
    /// </summary>
    public string About { get; set; }
}