using webapi.Dao.Models.Enums.Statistics;
using webapi.Models.TextsStatistics;

namespace webapi.Services.Abstract.TextsStatistics;

public interface ITextsStatisticsService
{
    /// <summary>
    /// Add texts statistics event
    /// </summary>
    Task<TextsStatisticsEvent> AddTextStatisticsEventAsync
    (
        TextsStatisticsEventType eventType,
        Guid textId,
        int? textPage,
        Guid? creatureId,
        string ip,
        string userAgent
    );

    /// <summary>
    /// Get texts complete reads count for given interval (start time included, end time excluded)
    /// </summary>
    Task<long> GetAllTextsCompleteReadsCountAsync(DateTime startTime, DateTime endTime);

    /// <summary>
    /// Get texts complete reads count for 24 hours
    /// </summary>
    Task<long> GetAllTextsCompleteReadsCountForLast24HoursAsync();
    
    /// <summary>
    /// Get the most popular texts IDs
    /// </summary>
    Task<IReadOnlyCollection<Guid>> GetMostPopularTextsIDsAsync(int skip, int take);
}