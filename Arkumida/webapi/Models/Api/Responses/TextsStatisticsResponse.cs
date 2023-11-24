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
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Information about stories on site
/// </summary>
public class TextsStatisticsResponse
{
    /// <summary>
    /// Total texts
    /// </summary>
    [JsonPropertyName("totalTexts")]
    public int TotalTexts { get; private set; }
    
    /// <summary>
    /// Stories read for last 24 hours
    /// </summary>
    [JsonPropertyName("readDuringLast24Hours")]
    public long ReadDuringLast24Hours { get; private set; }

    /// <summary>
    /// Last story was added this time
    /// </summary>
    [JsonPropertyName("lastAddTime")]
    public DateTime LastAddTime { get; private set; }

    public TextsStatisticsResponse
    (
        int totalTexts,
        long readDuringLast24Hours,
        DateTime lastAddTime
    )
    {
        if (totalTexts < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalTexts), "Total texts can't be negative.");
        }
        
        if (readDuringLast24Hours < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(readDuringLast24Hours), "Amount of texts read during last 24 hours can't be negative.");
        }

        TotalTexts = totalTexts;
        ReadDuringLast24Hours = readDuringLast24Hours;
        LastAddTime = lastAddTime;
    }
}