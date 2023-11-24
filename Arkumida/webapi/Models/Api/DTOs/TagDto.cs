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
using webapi.Dao.Models.Enums;
using webapi.Models.Enums;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Generic tag DTO, use it to create new tags
/// </summary>
public class TagDto : IdedEntityDto
{
    /// <summary>
    /// Tag subtype
    /// </summary>
    [JsonPropertyName("subtype")]
    public TagSubtype Subtype { get; set; }
    
    /// <summary>
    /// Tag name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// If true, than this tag represents a category
    /// </summary>
    [JsonPropertyName("isCategory")]
    public bool IsCategory { get; set; }

    /// <summary>
    /// If tag is category, then order it by this value
    /// </summary>
    [JsonPropertyName("categoryOrder")]
    public int CategoryOrder { get; set; }
    
    /// <summary>
    /// Category tag type - special types for category tags
    /// </summary>
    [JsonPropertyName("categoryTagType")]
    public CategoryTagType CategoryTagType { get; set; }
    
    /// <summary>
    /// If true, then tag is hidden (see TagDbo.cs for details)
    /// </summary>
    [JsonPropertyName("isHidden")]
    public bool IsHidden { get; set; }
    
    /// <summary>
    /// Machine-readable tag meaning
    /// </summary>
    [JsonPropertyName("meaning")]
    public TagMeaning Meaning { get; set; }
    
    public TagDto
    (
        Guid id,
        string furryReadableId,
        TagSubtype subtype,
        string name,
        bool isCategory,
        int categoryOrder,
        CategoryTagType categoryTagType,
        bool isHidden,
        TagMeaning meaning
    ) : base(id, furryReadableId)
    {
        Subtype = subtype;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tag name must be specified.", nameof(name));
        }
        Name = name;

        IsCategory = isCategory;
        CategoryOrder = categoryOrder;
        CategoryTagType = categoryTagType;
        IsHidden = isHidden;
        Meaning = meaning;
    }

    /// <summary>
    /// Convert to tag model
    /// </summary>
    /// <returns></returns>
    public Tag ToTag()
    {
        return new Tag()
        {
            Id = Id,
            FurryReadableId = FurryReadableId,
            Subtype = Subtype,
            Name = Name,
            IsCategory = IsCategory,
            CategoryOrder = CategoryOrder,
            TextsCount = 0,
            CategoryTagType = CategoryTagType,
            SizeCategory = TagSizeCategory.Cat0,
            IsHidden = IsHidden,
            Meaning = Meaning
        };
    }
}