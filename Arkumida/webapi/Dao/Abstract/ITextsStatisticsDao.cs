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

    #endregion
}