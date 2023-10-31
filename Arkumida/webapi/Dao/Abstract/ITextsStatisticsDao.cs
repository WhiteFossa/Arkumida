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
    /// Get all texts reads count since (including) given time
    /// </summary>
    Task<long> GetAllTextsReadsCountSinceTimeAsync(DateTime startTime);

    #endregion
}