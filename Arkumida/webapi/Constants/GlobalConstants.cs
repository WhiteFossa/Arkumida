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

namespace webapi.Constants;

/// <summary>
/// Store global constants here
/// </summary>
public static class GlobalConstants
{
    #region Requests-related

    /// <summary>
    /// Minimum displayNamePart length for UsersController.FindCreaturesByDisplayNamePartAsync() method. If length is less, then empty collection will be returned 
    /// </summary>
    public const int MinFindCreaturesByDisplayNamePartPartLength = 3;

    #endregion

    #region Parallelism-related

    /// <summary>
    /// Parallelism degree when giving creatures role User on startup
    /// </summary>
    public const int AddingUserRoleToCreaturesParallelismLevel = 12;

    #endregion
}