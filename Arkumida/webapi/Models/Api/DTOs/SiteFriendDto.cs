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
/// Information about one site friend
/// </summary>
public class SiteFriendDto
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }
    
    /// <summary>
    /// Friend's name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }
    
    /// <summary>
    /// Friend's resource URL
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; }
    
    /// <summary>
    /// Friend's resource title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }

    public SiteFriendDto
    (
        Guid id,
        string name,
        string url,
        string title
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Friend name mustn't be empty.", nameof(name));
        }
        
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Friend URL mustn't be empty.", nameof(url));
        }
        
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Friend title mustn't be empty.", nameof(title));
        }

        Id = id;
        Name = name;
        Url = url;
        Title = title;
    }
}