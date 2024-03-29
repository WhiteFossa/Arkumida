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

    public async Task<IReadOnlyCollection<TextsStatisticsEventDbo>> GetOrderedEventsByTextIdAsync(Guid textId, IReadOnlyCollection<TextsStatisticsEventType> filterByTypes = null)
    {
        var eventsQuery = _dbContext
            .TextsStatisticsEvents
            .Include(tse => tse.CausedByCreature)
            .Where(tse => tse.Text.Id == textId);

        if (filterByTypes != null)
        {
            eventsQuery = eventsQuery
                .Where(tse => filterByTypes.Contains(tse.Type));
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

    public async Task<IReadOnlyCollection<Guid>> GetMostPopularTextsIDsAsync(int skip, int take)
    {
        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), "Skip must not be negative.");
        }

        if (take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(take), "Take must be positive.");
        }

        return _dbContext
            .TextsStatisticsEvents
            .Where(tse => tse.Type == TextsStatisticsEventType.TextReadCompleted)
            .GroupBy(tse => tse.Text.Id)
            .ToDictionary(g => g.Key, g => g.LongCount())
            .OrderByDescending(di => di.Value)
            .Skip(skip)
            .Take(take)
            .Select(di => di.Key)
            .ToList();
            
    }

    public async Task<Dictionary<Guid, long>> GetTextsReadsCountAsync(IReadOnlyCollection<Guid> textsIds)
    {
        var result = await _dbContext
            .TextsStatisticsEvents
            .Where(tse => textsIds.Contains(tse.Text.Id))
            .Where(tse => tse.Type == TextsStatisticsEventType.TextReadCompleted)
            .GroupBy(tse => tse.Text.Id)
            .ToDictionaryAsync(g => g.Key, g => g.LongCount());
        
        // Injecting zeros
        foreach (var textId in textsIds)
        {
            result.TryAdd(textId, 0);
        }

        return result;
    }

    public async Task<Dictionary<TextsStatisticsEventType, long>> GetEventsCountsAsync(Guid textId, Guid? creatureId, IReadOnlyCollection<TextsStatisticsEventType> eventsTypes)
    {
        var query = _dbContext
            .TextsStatisticsEvents
            .Where(tse => tse.Text.Id == textId);

        if (creatureId.HasValue)
        {
            query = query
                .Where(tse => tse.CausedByCreature.Id == creatureId.Value);
        }
        else
        {
            query = query
                .Where(tse => tse.CausedByCreature == null);
        }

        query = query
            .Where(tse => eventsTypes.Contains(tse.Type));

        var result = await query
            .GroupBy(tse => tse.Type)
            .ToDictionaryAsync(g => g.Key, g => g.LongCount());

        foreach (var eventType in eventsTypes)
        {
            result.TryAdd(eventType, 0);
        }

        return result;
    }

    public async Task<Dictionary<TextsStatisticsEventType, long>> GetEventsCountsForAllCreaturesAsync(Guid textId, IReadOnlyCollection<TextsStatisticsEventType> eventsTypes)
    {
        var result = await _dbContext
            .TextsStatisticsEvents
            .Where(tse => tse.Text.Id == textId)
            .Where(tse => eventsTypes.Contains(tse.Type))
            .GroupBy(tse => tse.Type)
            .ToDictionaryAsync(g => g.Key, g => g.LongCount());
        
        foreach (var eventType in eventsTypes)
        {
            result.TryAdd(eventType, 0);
        }

        return result;
    }
}