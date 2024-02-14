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
using webapi.Models.Api.DTOs.Forum.Infos;

namespace webapi.Models.Api.Responses.Forum;

/// <summary>
/// Response with topic metadata
/// </summary>
public class ForumTopicInfoResponse
{
    /// <summary>
    /// Topic metadata
    /// </summary>
    [JsonPropertyName("forumTopicInfo")]
    public ForumTopicInfoDto ForumTopicInfo { get; }

    public ForumTopicInfoResponse
    (
        ForumTopicInfoDto forumTopicInfo
    )
    {
        ForumTopicInfo = forumTopicInfo ?? throw new ArgumentNullException(nameof(forumTopicInfo), "Forum topic metadata must be populated!");
    }
}