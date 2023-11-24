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
/// Machine-readable tag meaning
/// </summary>
public enum TagMeaning
{
    /// <summary>
    /// We can't tell anything specific about this tag
    /// </summary>
    Unspecified = 0,
    
    /// <summary>
    /// This tag marks stories category
    /// </summary>
    TypeStories = 1,
    
    /// <summary>
    /// This tag marks novels category
    /// </summary>
    TypeNovels = 2,
    
    /// <summary>
    /// This tag marks poetry category
    /// </summary>
    TypePoetry = 3,
    
    /// <summary>
    /// This tag marks comics category
    /// </summary>
    TypeComics = 4,

    /// <summary>
    /// Contest text (special type)
    /// </summary>
    SpecTypeContest = 5,
    
    /// <summary>
    /// Poor-quality text (special type)
    /// </summary>
    SpecTypeSandbox = 6,
    
    /// <summary>
    /// Snuff (special type)
    /// </summary>
    SpecTypeSnuff = 7,
    
    /// <summary>
    /// MLP
    /// </summary>
    MLP = 8
}