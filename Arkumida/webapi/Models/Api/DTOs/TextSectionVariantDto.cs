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

namespace webapi.Models.Api.DTOs;

/// <summary>
/// DTO for text section variant
/// </summary>
public class TextSectionVariantDto
{
    /// <summary>
    /// Variant ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Variant content (use it for variant creation, not for display)
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    /// <summary>
    /// Variant, parsed to elements. Use it for display.
    /// </summary>
    [JsonPropertyName("elements")]
    public IReadOnlyCollection<TextElementDto> Elements { get; set; }

    /// <summary>
    /// Variant creation time
    /// </summary>
    [JsonPropertyName("creationTime")]
    public DateTime CreationTime { get; set; }

    public TextSectionVariantDto()
    {
        
    }
    
    public TextSectionVariantDto
    (
        Guid id,
        string content,
        IReadOnlyCollection<TextElementDto> elements,
        DateTime creationTime
    )
    {
        Id = id;
        Content = content; // It may be empty
        Elements = elements ?? throw new ArgumentNullException(nameof(elements), "Elements collection must not be null.");
        CreationTime = creationTime;
    }

    /// <summary>
    /// To business-logic model
    /// </summary>
    public TextSectionVariant ToTextSectionVariant()
    {
        return new TextSectionVariant()
        {
            Id = Id,
            Content = Content,
            CreationTime = CreationTime
        };
    }
}