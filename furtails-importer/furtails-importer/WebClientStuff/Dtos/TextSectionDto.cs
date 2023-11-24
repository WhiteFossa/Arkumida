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

namespace furtails_importer.WebClientStuff.Dtos;

public class TextSectionDto
{
    /// <summary>
    /// Section ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Text in original language, usually in English. If this is Russian text, then original text is empty and the single variant
    /// contains actual text
    /// </summary>
    [JsonPropertyName("originalText")]
    public string OriginalText { get; set; }

    /// <summary>
    /// Sections are ordered by this field (to get a meaningful text)
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }
    
    /// <summary>
    /// Translation variants
    /// </summary>
    [JsonPropertyName("variants")]
    public IList<TextSectionVariantDto> Variants { get; set; }
}