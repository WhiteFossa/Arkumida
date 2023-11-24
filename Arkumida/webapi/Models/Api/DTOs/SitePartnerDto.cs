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
/// Site partners banners
/// </summary>
public class SitePartnerDto
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }
    
    /// <summary>
    /// Partner URL
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; }

    /// <summary>
    /// Link title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }
    
    /// <summary>
    /// Banner URL (relative, mostly probably)
    /// </summary>
    [JsonPropertyName("bannerUrl")]
    public string BannerUrl { get; }

    /// <summary>
    /// Alternative text for banner image
    /// </summary>
    [JsonPropertyName("bannerAlt")]
    public string BannerAlt { get; }

    public SitePartnerDto
    (
        Guid id,
        string url,
        string title,
        string bannerUrl,
        string bannerAlt
    )
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Partner URL mustn't be empty.", nameof(url));
        }
        
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Partner link title mustn't be empty.", nameof(title));
        }
        
        if (string.IsNullOrWhiteSpace(bannerUrl))
        {
            throw new ArgumentException("Partner banner URL mustn't be empty.", nameof(bannerUrl));
        }
        
        if (string.IsNullOrWhiteSpace(bannerAlt))
        {
            throw new ArgumentException("Partner banner alternative text mustn't be empty.", nameof(bannerAlt));
        }

        Id = id;
        Url = url;
        Title = title;
        BannerUrl = bannerUrl;
        BannerAlt = bannerAlt;
    }
}