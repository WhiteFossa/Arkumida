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

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with a link to Telegram group
/// </summary>
public class TelegramGroupResponse
{
    /// <summary>
    /// Group URL
    /// </summary>
    [JsonPropertyName("groupUrl")]
    public string GroupUrl { get; }
    
    /// <summary>
    /// Group title
    /// </summary>
    [JsonPropertyName("groupTitle")]
    public string GroupTitle { get; }

    public TelegramGroupResponse
    (
        string groupUrl,
        string groupTitle
    )
    {
        if (string.IsNullOrWhiteSpace(groupUrl))
        {
            throw new ArgumentException("Group URL must not be empty.", nameof(groupUrl));
        }
        
        if (string.IsNullOrWhiteSpace(groupTitle))
        {
            throw new ArgumentException("Group title must not be empty.", nameof(groupTitle));
        }

        GroupUrl = groupUrl;
        GroupTitle = groupTitle;
    }
}