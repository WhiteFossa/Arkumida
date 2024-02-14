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

namespace webapi.Models.Api.DTOs.Forum.Infos;

/// <summary>
/// DTO with forum topic metadata
/// </summary>
public class ForumTopicInfoDto
{
    /// <summary>
    /// Topic ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; private set; }

    /// <summary>
    /// Topic name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; private set; }

    /// <summary>
    /// Topic description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; private set; }

    /// <summary>
    /// Messages count
    /// </summary>
    [JsonPropertyName("messagesCount")]
    public int MessagesCount { get; private set; }

    /// <summary>
    /// First message in topic
    /// </summary>
    [JsonPropertyName("firstMessage")]
    public ForumMessageDto FirstMessage { get; private set; }

    /// <summary>
    /// Last message in topic
    /// </summary>
    [JsonPropertyName("lastMessage")]
    public ForumMessageDto LastMessage { get; private set; }

    /// <summary>
    /// If this field is not null, then this topic is comments topic for given text
    /// </summary>
    [JsonPropertyName("commentsForText")]
    public TextDto CommentsForText { get; private set; }

    public ForumTopicInfoDto
    (
        Guid id,
        string name,
        string description,
        int messagesCount,
        ForumMessageDto firstMessage,
        ForumMessageDto lastMessage,
        TextDto commentsForText
    )
    {
        Id = id;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Topic name must be populated!", nameof(name));
        }

        Name = name;

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Topic description must be populated!", nameof(description));
        }

        Description = description;

        if (messagesCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(messagesCount), $"Messages count is { messagesCount }, but have to be non-negative!");
        }

        MessagesCount = messagesCount;

        FirstMessage = firstMessage ?? throw new ArgumentNullException(nameof(firstMessage), "First message mustn't be null!");
        LastMessage = lastMessage ?? throw new ArgumentNullException(nameof(lastMessage), "Last message mustn't be null!");
        CommentsForText = commentsForText; // May be null for non-comments topic
    }
}