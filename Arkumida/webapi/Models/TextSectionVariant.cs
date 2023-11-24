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

using webapi.Models.Api.DTOs;
using webapi.Services.Abstract;

namespace webapi.Models;

/// <summary>
/// Text section variant, part of section
/// </summary>
public class TextSectionVariant
{
    /// <summary>
    /// Variant ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Variant content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Variant creation time
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// To DTO
    /// </summary>
    public TextSectionVariantDto ToDto(IReadOnlyCollection<TextFile> textFiles, ITextUtilsService textUtilsService)
    {
        return new TextSectionVariantDto
        (
            Id,
            string.Empty, // We don't need to pass content to frontend
            textUtilsService.ParseTextToElements(Content, textFiles),
            CreationTime
        );
    }
}