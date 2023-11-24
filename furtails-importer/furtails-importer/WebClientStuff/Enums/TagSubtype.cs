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
/// Subtype for tags (type of action, species and so on)
/// </summary>
public enum TagSubtype
{
    /// <summary>
    /// Participants, e.g. M/F
    /// </summary>
    Participants = 1,
    
    /// <summary>
    /// Species, e.g. earlybeast
    /// </summary>
    Species = 2,
    
    /// <summary>
    /// Setting, e.g. SciFi
    /// </summary>
    Setting = 3,
    
    /// <summary>
    /// Actions, e.g. erotic tickling
    /// </summary>
    Actions = 4,
    
    /// <summary>
    /// Category tag
    /// </summary>
    Category = 5
}