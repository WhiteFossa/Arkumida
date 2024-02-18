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
using webapi.Models.Api.DTOs.Forum;

namespace webapi.Models.Api.Responses.Forum;

/// <summary>
/// Response with messages from topic
/// </summary>
public class ForumTopicMessagesResponse
{
    /// <summary>
    /// Topic ID
    /// </summary>
    [JsonPropertyName("topicId")]
    public Guid TopicId { get; }

    /// <summary>
    /// Amount of messages skipped
    /// </summary>
    [JsonPropertyName("skip")]
    public int Skip { get; }

    /// <summary>
    /// Amount of messages requested
    /// </summary>
    [JsonPropertyName("take")]
    public int Take { get; }

    /// <summary>
    /// Actually returned messages (messages amount may be less than Take)
    /// </summary>
    [JsonPropertyName("messages")]
    public IReadOnlyCollection<ForumMessageDto> Messages { get; }

    public ForumTopicMessagesResponse
    (
        Guid topicId,
        int skip,
        int take,
        IReadOnlyCollection<ForumMessageDto> messages
    )
    {
        TopicId = topicId;

        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), skip, "Skip must be non-negative!");
        }

        Skip = skip;

        if (take < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(take), take, "Take must be 1 or greater!");
        }

        Take = take;

        Messages = messages ?? throw new ArgumentNullException(nameof(messages), "Messages mustn't be null!");
    }
}