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
/// DTO, representing a link
/// </summary>
public class LinkDto
{
    /// <summary>
    /// URL
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; }
    
    /// <summary>
    /// Link text
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; }
    
    /// <summary>
    /// Link title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }

    public LinkDto
    (
        string url,
        string text,
        string title
    )
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Link URL mustn't be empty.", nameof(url));
        }
        
        // Text is not being checked for emptiness because children (like ImagedLinkDto) may have empty text
        
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Link title mustn't be empty.", nameof(title));
        }

        Url = url;
        Text = text;
        Title = title;
    }
}