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
using webapi.Dao.Models.Enums.Statistics;
using webapi.Models.Api.DTOs.Creatures;

namespace webapi.Models.Api.DTOs.TextsStatistics;

/// <summary>
/// Vote for text
/// </summary>
public class TextVoteEventDto
{
    /// <summary>
    /// Vote ID (equal to TextsStatisticsEventDbo.Id)
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; private set; }
    
    /// <summary>
    /// When vote occured
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; private set; }
    
    /// <summary>
    /// Event type. Must be one of the next: Like, Unlike, Dislike, Undislike
    /// </summary>
    [JsonPropertyName("type")]
    public TextsStatisticsEventType Type { get; private set; }

    /// <summary>
    /// If true, then voted creature is hidden. Always true for Like / Unlike votes
    /// </summary>
    [JsonPropertyName("isCreatureHidden")]
    public bool IsCreatureHidden { get; private set; }
    
    /// <summary>
    /// Creature, who voted. If creature is hidden, then here will be null
    /// </summary>
    [JsonPropertyName("creature")]
    public CreatureWithProfileDto Creature { get; private set; }

    public TextVoteEventDto
    (
        Guid id,
        DateTime timestamp,
        TextsStatisticsEventType type,
        bool isCreatureHidden,
        CreatureWithProfileDto creature
    )
    {
        Id = id;
        Timestamp = timestamp;

        if (type != TextsStatisticsEventType.Like
            &&
            type != TextsStatisticsEventType.UnLike
            &&
            type != TextsStatisticsEventType.Dislike
            &&
            type != TextsStatisticsEventType.UnDislike)
        {
            throw new ArgumentException($"Incorrect vote event type: {type}", nameof(type));
        }

        Type = type;

        IsCreatureHidden = isCreatureHidden;

        if (!isCreatureHidden && creature == null)
        {
            throw new ArgumentNullException(nameof(creature), "Creature mustn't be null if creature isn't hidden.");
        }
        
        Creature = !isCreatureHidden ? creature : null;
    }
}