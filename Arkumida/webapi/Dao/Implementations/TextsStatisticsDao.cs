using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums.Statistics;

namespace webapi.Dao.Implementations;

public class TextsStatisticsDao : ITextsStatisticsDao
{
    private readonly MainDbContext _dbContext;

    public TextsStatisticsDao
    (
        MainDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }
    
    public async Task<TextsStatisticsEventDbo> InsertEventAsync(TextsStatisticsEventDbo statisticsEvent)
    {
        _ = statisticsEvent ?? throw new ArgumentNullException(nameof(statisticsEvent), "Statistics event must not be null!");

        statisticsEvent.Text = await _dbContext.Texts.SingleAsync(t => t.Id == statisticsEvent.Text.Id);
        statisticsEvent.CausedByCreature = statisticsEvent.CausedByCreature != null ? await _dbContext.Users.SingleAsync(u => u.Id == statisticsEvent.CausedByCreature.Id) : null;

        await _dbContext
            .TextsStatisticsEvents
            .AddAsync(statisticsEvent);

        await _dbContext.SaveChangesAsync();

        return statisticsEvent;
    }

    public async Task<IReadOnlyCollection<TextsStatisticsEventDbo>> GetOrderedEventsByTextIdAsync(Guid textId, TextsStatisticsEventType? filterByType = null)
    {
        var eventsQuery = _dbContext
            .TextsStatisticsEvents
            .Where(tse => tse.Text.Id == textId);

        if (filterByType.HasValue)
        {
            eventsQuery = eventsQuery
                .Where(tse => tse.Type == filterByType.Value);
        }

        return await eventsQuery
            .OrderBy(tse => tse.Timestamp)
            .ToListAsync();
    }

    public async Task<long> GetAllTextsCompleteReadsCountAsync(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
        {
            throw new ArgumentOutOfRangeException(nameof(endTime), "End time must be greater then start time!");
        }
        
        return await _dbContext
            .TextsStatisticsEvents
            .Where(tse => tse.Type == TextsStatisticsEventType.TextReadCompleted)
            .Where(tse => tse.Timestamp >= startTime)
            .Where(tse => tse.Timestamp < endTime)
            .LongCountAsync();
    }
}