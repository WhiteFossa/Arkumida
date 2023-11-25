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

using webapi.Dao.Models.Enums.Statistics;
using webapi.Models.Api.DTOs.TextsStatistics;
using webapi.Models.Creatures;

namespace webapi.Models.TextsStatistics;

/// <summary>
/// Texts statistics event model
/// </summary>
public class TextsStatisticsEvent
{
    /// <summary>
    /// Event ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// When event occured
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Event is related to this text
    /// </summary>
    public Text Text { get; set; }
    
    /// <summary>
    /// Event is related to this text page
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Event type
    /// </summary>
    public TextsStatisticsEventType Type { get; set; }
    
    /// <summary>
    /// If caused by registered creature, then shi will be here, otherwise here will be null
    /// </summary>
    public Creature CausedByCreature { get; set; }

    /// <summary>
    /// IPv4 or IPv6 of creature, who caused the event
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// Useragent of creature, who caused the event
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// Convert to DTO
    /// </summary>
    public TextsStatisticsEventDto ToDto()
    {
        return new TextsStatisticsEventDto()
        {
            Id = Id,
            Timestamp = Timestamp,
            TextId = Text.Id,
            Page = Page,
            Type = Type,
            CreatureId = CausedByCreature?.Id,
            Ip = Ip,
            UserAgent = UserAgent
        };
    }
}