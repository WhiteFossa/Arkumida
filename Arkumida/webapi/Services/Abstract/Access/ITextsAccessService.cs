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

namespace webapi.Services.Abstract.Access;

/// <summary>
/// Methods to check access to texts and related stuff
/// </summary>
public interface ITextsAccessService
{
    /// <summary>
    /// Is creature related to text?
    /// By "related" we mean:
    /// - Publisher
    /// - Authors
    /// - Translators
    /// </summary>
    Task<bool> IsCreatureRelatedToTextAsync(Guid textId, Guid creatureId);
    
    /// <summary>
    /// Is likes/dislikes history visible to creature?
    /// </summary>
    Task<bool> IsVotesHistoryVisibleAsync(Guid textId, Guid? creatureId);

    /// <summary>
    /// Is creature can vote for text?
    /// </summary>
    Task<bool> IsCanVoteForTextAsync(Guid textId, Guid? creatureId);
}