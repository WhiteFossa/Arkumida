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

namespace webapi.Dao.Models.Enums.Statistics;

/// <summary>
/// Possible event type for statistics log
/// </summary>
public enum TextsStatisticsEventType
{
    /// <summary>
    /// Text page was read
    /// </summary>
    PageRead = 0,
    
    /// <summary>
    /// Text was liked
    /// </summary>
    Like = 1,
    
    /// <summary>
    /// Text was disliked
    /// </summary>
    Dislike = 2,
    
    /// <summary>
    /// Text was completely read by creature
    /// </summary>
    TextReadCompleted = 3,
    
    /// <summary>
    /// Text like was removed
    /// </summary>
    UnLike = 4,
    
    /// <summary>
    /// Text dislike was removed
    /// </summary>
    UnDislike = 5
}