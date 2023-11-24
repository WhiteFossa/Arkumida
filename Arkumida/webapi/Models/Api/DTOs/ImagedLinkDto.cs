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
/// Link with image
/// </summary>
public class ImagedLinkDto : LinkDto
{
    /// <summary>
    /// Image URL
    /// </summary>
    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; }
    
    /// <summary>
    /// Image alt
    /// </summary>
    [JsonPropertyName("imageAlt")]
    public string ImageAlt { get; }
    
    /// <summary>
    /// Image class
    /// </summary>
    [JsonPropertyName("imageClass")]
    public string ImageClass { get; }
    
    public ImagedLinkDto(string url, string text, string title, string imageUrl, string imageAlt, string imageClass) : base(url, text, title)
    {
        // We dont check for Image URL, ImageAlt and ImageClass emptiness because it can be empty (if link have no image for example)
        ImageUrl = imageUrl;
        ImageAlt = imageAlt;
        ImageClass = imageClass;
    }
}