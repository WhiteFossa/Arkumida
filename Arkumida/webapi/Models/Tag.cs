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

using webapi.Dao.Models.Enums;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;

namespace webapi.Models;

/// <summary>
/// Tag (can be used as a text tag and as a category)
/// </summary>
public class Tag : IdedEntity
{
    /// <summary>
    /// Tag subtype
    /// </summary>
    public TagSubtype Subtype { get; set; }
    
    /// <summary>
    /// Tag name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// If true, than this tag represents a category
    /// </summary>
    public bool IsCategory { get; set; }

    /// <summary>
    /// If tag is category, then order it by this value
    /// </summary>
    public int CategoryOrder { get; set; }

    /// <summary>
    /// How much texts are marked with this tag
    /// </summary>
    public int TextsCount { get; set; }

    /// <summary>
    /// Category tag type - special types for category tags
    /// </summary>
    public CategoryTagType CategoryTagType { get; set; }

    /// <summary>
    /// Tag size category
    /// </summary>
    public TagSizeCategory SizeCategory { get; set; }

    /// <summary>
    /// If true, then tag is hidden (see TagDbo.cs for details)
    /// </summary>
    public bool IsHidden { get; set; }
    
    /// <summary>
    /// Machine-readable tag meaning
    /// </summary>
    public TagMeaning Meaning { get; set; }
    
    /// <summary>
    /// Generate TextTagDto from tag
    /// </summary>
    public TextTagDto ToTextTagDto()
    {
        return new TextTagDto(Id, FurryReadableId, Name, IsCategory, SizeCategory, Meaning);
    }

    /// <summary>
    /// Generate CategoryTagDto from tag
    /// </summary>
    public CategoryTagDto ToCategoryTagDto()
    {
        if (!IsCategory)
        {
            throw new InvalidOperationException($"Tag with ID={Id} is not a category tag.");
        }
        
        return new CategoryTagDto(Id, FurryReadableId, Name, TextsCount, CategoryTagType);
    }

    /// <summary>
    /// Generate TagDto from tag model
    /// </summary>
    public TagDto ToTagDto()
    {
        return new TagDto(Id, FurryReadableId, Subtype, Name, IsCategory, CategoryOrder, CategoryTagType, IsHidden, Meaning);
    }
}