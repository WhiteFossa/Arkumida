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
using webapi.Dao.Models.Enums;

namespace webapi.Dao.Models;

/// <summary>
/// Tag, database object
/// </summary>
public class TagDbo
{
    /// <summary>
    /// Tag ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Furry readable ID
    /// </summary>
    public string FurryReadableId { get; set; }

    /// <summary>
    /// Tag subtype
    /// </summary>
    public TagSubtype Subtype { get; set; }

    /// <summary>
    /// Tag name, i.e. tag itself
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// If true, then this tag is category
    /// Some categories are visible in text tags list (like "M/F"), some aren't (like "Novels") so we can't use this flag to hide tags
    /// </summary>
    public bool IsCategory { get; set; }

    /// <summary>
    /// If tag is category, then order it by this value
    /// </summary>
    public int CategoryOrder { get; set; }

    /// <summary>
    /// Category type
    /// </summary>
    public CategoryTagType CategoryType { get; set; }

    /// <summary>
    /// Tag's texts
    /// </summary>
    public IList<TextDbo> Texts { get; set; }

    /// <summary>
    /// If true, then tag is hidden from:
    /// - text tags list
    /// - tags cloud
    /// Such tag is ignored when calculating the size categories of tags
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    /// Machine-readable tag meaning
    /// </summary>
    public TagMeaning Meaning { get; set; }

}