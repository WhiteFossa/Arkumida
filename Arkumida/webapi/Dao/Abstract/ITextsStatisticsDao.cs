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

using webapi.Dao.Models;
using webapi.Dao.Models.Enums.Statistics;

namespace webapi.Dao.Abstract;

/// <summary>
/// DAO to work with texts statistics
/// </summary>
public interface ITextsStatisticsDao
{
    #region Create

    /// <summary>
    /// Insert statistics event into DB
    /// </summary>
    Task<TextsStatisticsEventDbo> InsertEventAsync(TextsStatisticsEventDbo statisticsEvent);

    #endregion
    
    #region Get

    /// <summary>
    /// Get all events for given text, ordered by timestamp (earlier first) 
    /// </summary>
    Task<IReadOnlyCollection<TextsStatisticsEventDbo>> GetOrderedEventsByTextIdAsync(Guid textId, TextsStatisticsEventType? filterByType = null);

    /// <summary>
    /// Get all texts reads count since (including) start time, till (excluding) end time
    /// </summary>
    Task<long> GetAllTextsCompleteReadsCountAsync(DateTime startTime, DateTime endTime);

    /// <summary>
    /// Get the most popular texts IDs
    /// </summary>
    Task<IReadOnlyCollection<Guid>> GetMostPopularTextsIDsAsync(int skip, int take);

    /// <summary>
    /// Get reads count for given texts. If text was never read it will return 0, i.e. item wouldn't be absent
    /// </summary>
    Task<Dictionary<Guid, long>> GetTextsReadsCountAsync(IReadOnlyCollection<Guid> textsIds);

    /// <summary>
    /// Returns given events types counts for given creature and text 
    /// </summary>
    Task<Dictionary<TextsStatisticsEventType, long>> GetEventsCountsAsync(Guid textId, Guid? creatureId, IReadOnlyCollection<TextsStatisticsEventType> eventsTypes);
    
    /// <summary>
    /// Returns given events types counts for given text
    /// </summary>
    Task<Dictionary<TextsStatisticsEventType, long>> GetEventsCountsForAllCreaturesAsync(Guid textId, IReadOnlyCollection<TextsStatisticsEventType> eventsTypes);

    #endregion
}