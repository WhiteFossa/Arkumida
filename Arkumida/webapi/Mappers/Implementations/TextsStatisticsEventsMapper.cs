using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models.TextsStatistics;

namespace webapi.Mappers.Implementations;

public class TextsStatisticsEventsMapper : ITextsStatisticsEventsMapper
{
    private readonly ITextsMapper _textsMapper;
    private readonly ICreaturesMapper _creaturesMapper;

    public TextsStatisticsEventsMapper
    (
        ITextsMapper textsMapper,
        ICreaturesMapper creaturesMapper
    )
    {
        _textsMapper = textsMapper;
        _creaturesMapper = creaturesMapper;
    }
    
    public IReadOnlyCollection<TextsStatisticsEvent> Map(IEnumerable<TextsStatisticsEventDbo> statisticsEvents)
    {
        if (statisticsEvents == null)
        {
            return null;
        }

        return statisticsEvents.Select(se => Map(se)).ToList();
    }

    public TextsStatisticsEvent Map(TextsStatisticsEventDbo statisticsEvent)
    {
        if (statisticsEvent == null)
        {
            return null;
        }

        return new TextsStatisticsEvent()
        {
            Id = statisticsEvent.Id,
            Timestamp = statisticsEvent.Timestamp,
            Text = _textsMapper.Map(statisticsEvent.Text),
            Type = statisticsEvent.Type,
            CausedByCreature = _creaturesMapper.Map(statisticsEvent.CausedByCreature),
            Ip = statisticsEvent.Ip,
            UserAgent = statisticsEvent.UserAgent
        };
    }

    public TextsStatisticsEventDbo Map(TextsStatisticsEvent statisticsEvent)
    {
        if (statisticsEvent == null)
        {
            return null;
        }

        return new TextsStatisticsEventDbo()
        {
            Id = statisticsEvent.Id,
            Timestamp = statisticsEvent.Timestamp,
            Text = _textsMapper.Map(statisticsEvent.Text),
            Type = statisticsEvent.Type,
            CausedByCreature = _creaturesMapper.Map(statisticsEvent.CausedByCreature),
            Ip = statisticsEvent.Ip,
            UserAgent = statisticsEvent.UserAgent
        };
    }

    public IReadOnlyCollection<TextsStatisticsEventDbo> Map(IEnumerable<TextsStatisticsEvent> statisticsEvents)
    {
        if (statisticsEvents == null)
        {
            return null;
        }

        return statisticsEvents.Select(se => Map(se)).ToList();
    }
}