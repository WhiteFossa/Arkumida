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

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Tag for category
/// </summary>
public class CategoryTagDto : IdedEntityDto
{
    /// <summary>
    /// Tag title
    /// </summary>
    [JsonPropertyName("tag")]
    public string Tag { get; private set; }

    /// <summary>
    /// How much texts are marked with this tag
    /// </summary>
    [JsonPropertyName("textsCount")]
    public int TextsCount { get; private set; }

    /// <summary>
    /// Special type for category tag
    /// </summary>
    [JsonPropertyName("type")]
    public CategoryTagType Type { get; private set; }

    public CategoryTagDto
    (
        Guid id,
        string furryReadableId,
        string tag,
        int textsCount,
        CategoryTagType type
    ) : base(id, furryReadableId)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            throw new ArgumentException("Tag must be populated.", nameof(tag));
        }
        Tag = tag;

        if (textsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(textsCount), "Texts count must be non-negative.");
        }
        TextsCount = textsCount;

        Type = type;
    }
}