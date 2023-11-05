using webapi.Dao.Models;
using webapi.Models.TextsStatistics;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Texts statistics events mapper
/// </summary>
public interface ITextsStatisticsEventsMapper
{
    IReadOnlyCollection<TextsStatisticsEvent> Map(IEnumerable<TextsStatisticsEventDbo> statisticsEvents);

    TextsStatisticsEvent Map(TextsStatisticsEventDbo statisticsEvent);

    TextsStatisticsEventDbo Map(TextsStatisticsEvent statisticsEvent);

    IReadOnlyCollection<TextsStatisticsEventDbo> Map(IEnumerable<TextsStatisticsEvent> statisticsEvents);
}