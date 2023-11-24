#region License
// Furtails Importer - Importer from furtails.pw database to Arkumida
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

namespace furtails_importer.WebClientStuff.Enums;

/// <summary>
/// Possible category tag types
/// </summary>
public enum CategoryTagType
{
    /// <summary>
    /// Normal category tag
    /// </summary>
    Normal = 0,
    
    /// <summary>
    /// Snuff category, mark with pink and show warning
    /// </summary>
    Snuff = 1,
    
    /// <summary>
    /// Poor-quality texts, mark with gray and show warning
    /// </summary>
    Sandbox = 2,
    
    /// <summary>
    /// Contest texts, mark with yellow
    /// </summary>
    Contest = 3
}