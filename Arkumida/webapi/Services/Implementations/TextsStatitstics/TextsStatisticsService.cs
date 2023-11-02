using Microsoft.AspNetCore.Identity;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums.Statistics;
using webapi.Mappers.Abstract;
using webapi.Mappers.Implementations;
using webapi.Models;
using webapi.Models.TextsStatistics;
using webapi.Services.Abstract.TextsStatistics;

namespace webapi.Services.Implementations.TextsStatitstics;

public class TextsStatisticsService : ITextsStatisticsService
{
    private readonly ITextsStatisticsDao _textsStatisticsDao;
    private readonly ITextsStatisticsEventsMapper _textsStatisticsEventsMapper;
    private readonly UserManager<CreatureDbo> _userManager;
    private readonly ICreaturesMapper _creaturesMapper;

    public TextsStatisticsService
    (
        ITextsStatisticsDao textsStatisticsDao,
        ITextsStatisticsEventsMapper textsStatisticsEventsMapper,
        UserManager<CreatureDbo> userManager,
        ICreaturesMapper creaturesMapper
    )
    {
        _textsStatisticsDao = textsStatisticsDao;
        _textsStatisticsEventsMapper = textsStatisticsEventsMapper;
        _userManager = userManager;
        _creaturesMapper = creaturesMapper;
    }
    
    public async Task<TextsStatisticsEvent> AddTextStatisticsEventAsync(TextsStatisticsEventType eventType, Guid textId, Guid? creatureId, string ip, string userAgent)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            throw new ArgumentException("IP must be specified!", nameof(ip));
        }

        if (string.IsNullOrWhiteSpace(userAgent))
        {
            throw new ArgumentException("User agent must be specified!", nameof(userAgent));
        }

        CreatureDbo causedByCreature = null;
        if (creatureId.HasValue)
        {
            causedByCreature = await _userManager.FindByIdAsync(creatureId.Value.ToString()); 
        }
        
        var statisticsEvent = new TextsStatisticsEvent()
        {
            Timestamp = DateTime.UtcNow,
            Text = new Text() { Id = textId },
            Type = eventType,
            CausedByCreature = _creaturesMapper.Map(causedByCreature),
            Ip = ip,
            UserAgent = userAgent
        };

        var statisticsEventDbo = _textsStatisticsEventsMapper.Map(statisticsEvent);
        var addedEvent = await _textsStatisticsDao.InsertEventAsync(statisticsEventDbo);

        return _textsStatisticsEventsMapper.Map(addedEvent);
    }
}