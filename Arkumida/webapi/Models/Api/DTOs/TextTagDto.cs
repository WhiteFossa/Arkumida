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
/// Tag for text
/// </summary>
public class TextTagDto : IdedEntityDto
{
    /// <summary>
    /// Tag title
    /// </summary>
    [JsonPropertyName("tag")]
    public string Tag { get; private set; }

    /// <summary>
    /// If true, then display it before hash
    /// </summary>
    [JsonPropertyName("isCategory")]
    public bool IsCategory { get; private set; }
    
    /// <summary>
    /// Tag size category
    /// </summary>
    [JsonPropertyName("sizeCategory")]
    public TagSizeCategory SizeCategory { get; private set; }

    /// <summary>
    /// Tag meaning
    /// </summary>
    [JsonPropertyName("meaning")]
    public TagMeaning Meaning { get; private set; }

    public TextTagDto
    (
        Guid id,
        string furryReadableId,
        string tag,
        bool isCategory,
        TagSizeCategory sizeCategory,
        TagMeaning meaning
    ) : base(id, furryReadableId)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            throw new ArgumentException("Tag must be populated.", nameof(tag));
        }
        Tag = tag;
        IsCategory = isCategory;
        SizeCategory = sizeCategory;
        Meaning = meaning;
    }
}