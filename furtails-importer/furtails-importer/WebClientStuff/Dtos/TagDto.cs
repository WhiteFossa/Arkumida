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

using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Enums;

namespace furtails_importer.WebClientStuff.Dtos;

public class TagDto
{
    /// <summary>
    /// Entity Id
    /// </summary>
    [JsonPropertyName("entityId")]
    public Guid Id { get; set; }

    /// <summary>
    /// Furry-readable ID (mostly for compatibility with old site)
    /// </summary>
    [JsonPropertyName("furryReadableId")]
    public string FurryReadableId { get; set; }
    
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
}