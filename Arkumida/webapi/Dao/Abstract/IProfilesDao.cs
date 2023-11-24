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
/// Interface for work with creatures profiles
/// </summary>
public interface IProfilesDao
{
    /// <summary>
    /// Create a profile
    /// </summary>
    Task<CreatureProfileDbo> CreateProfileAsync(CreatureProfileDbo profile);

    /// <summary>
    /// Update the profile
    /// </summary>
    Task<CreatureProfileDbo> UpdateProfileAsync(CreatureProfileDbo newProfile);
    
    /// <summary>
    /// Get creature profile
    /// </summary>
    Task<CreatureProfileDbo> GetProfileAsync(Guid creatureId);

    /// <summary>
    /// Mass get creatures profiles by IDs
    /// </summary>
    Task<IReadOnlyCollection<CreatureProfileDbo>> MassGetProfilesAsync(IReadOnlyCollection<Guid> creaturesIds);

    /// <summary>
    /// Return all creatures profiles, who's display names contain displayNamePart (case insensitive)
    /// </summary>
    Task<IReadOnlyCollection<CreatureProfileDbo>> FindCreaturesProfilesByDisplayNamePartAsync(string displayNamePart);

    /// <summary>
    /// Find creature by display name
    /// </summary>
    Task<CreatureProfileDbo> FindCreatureByDisplayNameAsync(string displayName);
}