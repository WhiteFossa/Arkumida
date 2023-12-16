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
/// Creature with profile
/// </summary>
public class CreatureWithProfile : Creature
{
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
    public IList<Avatar> Avatars { get; set; }

    /// <summary>
    /// Creature's current avatar
    /// </summary>
    public Avatar CurrentAvatar { get; set; }
    
    /// <summary>
    /// Information about creature (in the format, processable by ITextUtilsService.ParseTextToElements())
    /// </summary>
    public string About { get; set; }
    
    public CreatureWithProfile
    (
        Guid id,
        string login,
        string email,
        bool isPasswordChangeRequired,
        string oneTimePlaintextPassword,
        string displayName,
        IList<Avatar> avatars,
        Avatar currentAvatar,
        string about
    ) : base(id, login, email)
    {
        // All fields may be null (for example when creating a new text)
        IsPasswordChangeRequired = isPasswordChangeRequired;
        OneTimePlaintextPassword = oneTimePlaintextPassword;
        DisplayName = displayName;
        Avatars = avatars;
        CurrentAvatar = currentAvatar;
        About = about;
    }

    public new CreatureWithProfileDto ToDto()
    {
        return new CreatureWithProfileDto
        (
            Id,
            "not_ready",
            Login,
            Email,
            IsPasswordChangeRequired,
            DisplayName,
            Avatars?.Select(a => a.ToDto()).ToList(),
            CurrentAvatar?.ToDto(),
            About
        );
    }
}