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

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Creature with profile DTO
/// </summary>
public class CreatureWithProfileDto : CreatureDto
{
    /// <summary>
    /// If true, then creature must change hir password
    /// </summary>
    [JsonPropertyName("isPasswordChangeRequired")]
    public bool IsPasswordChangeRequired { get; set; }
    
    /// <summary>
    /// User's visible name
    /// </summary>
    [JsonPropertyName("name")]
    public string DisplayName { get; set; }
    
    /// <summary>
    /// Creature's avatars
    /// </summary>
    [JsonPropertyName("avatars")]
    public IReadOnlyCollection<AvatarDto> Avatars { get; set; }

    /// <summary>
    /// Creature's current avatar
    /// </summary>
    [JsonPropertyName("currentAvatar")]
    public AvatarDto CurrentAvatar { get; set; }
    
    /// <summary>
    /// Information about creature (in the format, processable by ITextUtilsService.ParseTextToElements())
    /// </summary>
    [JsonPropertyName("about")]
    public string About { get; set; }
    
    public CreatureWithProfileDto
    (
        Guid id,
        string furryReadableId,
        string login,
        string email,
        bool isPasswordChangeRequired,
        string displayName,
        IReadOnlyCollection<AvatarDto> avatars,
        AvatarDto currentAvatar,
        string about
    ) : base(id, furryReadableId, login, email)
    {
        // All fields may be null (during the text creation, for example)
        IsPasswordChangeRequired = isPasswordChangeRequired;
        DisplayName = displayName;
        Avatars = avatars;
        CurrentAvatar = currentAvatar;
        About = about;
    }

    public CreatureWithProfile ToCreatureWithProfile()
    {
        return new CreatureWithProfile
        (
            Id,
            Login,
            Email,
            false,
            string.Empty,
            DisplayName,
            Avatars?.Select(a => a.ToModel()).ToList(),
            CurrentAvatar?.ToModel(),
            About
        );
    }
}