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
/// Text page DTO
/// </summary>
public class TextPageDto
{
    /// <summary>
    /// Page ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Page number, order pages by this value
    /// </summary>
    [JsonPropertyName("number")]
    public int Number { get; set; }

    /// <summary>
    /// Text sections
    /// </summary>
    [JsonPropertyName("sections")]
    public IList<TextSectionDto> Sections { get; set; }

    public TextPageDto()
    {

    }

    public TextPageDto
    (
        Guid id,
        int number,
        IReadOnlyCollection<TextSectionDto> sections)
    {
        Id = id;
        Number = number;
        Sections = (sections ?? throw new ArgumentNullException(nameof(sections), "Sections must be populated!")).ToList();
    }

    public TextPage ToTextPage()
    {
        return new TextPage()
        {
            Id = Id,
            Number = Number,
            Sections = Sections.Select(s => s.ToTextSection()).ToList()
        };
    }

}