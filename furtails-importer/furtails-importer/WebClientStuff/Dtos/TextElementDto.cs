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

/// <summary>
/// Low level text element, all texts are lists of elements
/// </summary>
public class TextElementDto
{
    /// <summary>
    /// Element type
    /// </summary>
    [JsonPropertyName("type")]
    public TextElementType Type { get; set; }

    /// <summary>
    /// Element content, may be empty for contentless elements
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    /// <summary>
    /// Element parameters (as a set of strings)
    /// </summary>
    [JsonPropertyName("parameters")]
    public IReadOnlyCollection<string> Parameters { get; set; }

    public TextElementDto
    (
        TextElementType type,
        string content,
        IReadOnlyCollection<string> parameters)
    {
        Type = type;
        Content = content;
        Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters), "Parameters must not be empty!");
    }
}