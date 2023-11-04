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
}