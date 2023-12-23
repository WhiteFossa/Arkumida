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

using Microsoft.AspNetCore.Identity;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums.Statistics;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Api.DTOs.TextsStatistics;
using webapi.Models.TextsStatistics;
using webapi.Services.Abstract;
using webapi.Services.Abstract.TextsStatistics;

namespace webapi.Services.Implementations.TextsStatitstics;

public class TextsStatisticsService : ITextsStatisticsService
{
    private readonly ITextsStatisticsDao _textsStatisticsDao;
    private readonly ITextsStatisticsEventsMapper _textsStatisticsEventsMapper;
    private readonly UserManager<CreatureDbo> _userManager;
    private readonly ICreaturesMapper _creaturesMapper;
    private readonly ITextsDao _textsDao;
    private readonly IAccountsService _accountsService;

    public TextsStatisticsService
    (
        ITextsStatisticsDao textsStatisticsDao,
        ITextsStatisticsEventsMapper textsStatisticsEventsMapper,
        UserManager<CreatureDbo> userManager,
        ICreaturesMapper creaturesMapper,
        ITextsDao textsDao,
        IAccountsService accountsService
    )
    {
        _textsStatisticsDao = textsStatisticsDao;
        _textsStatisticsEventsMapper = textsStatisticsEventsMapper;
        _userManager = userManager;
        _creaturesMapper = creaturesMapper;
        _textsDao = textsDao;
        _accountsService = accountsService;
    }

    public async Task<TextsStatisticsEvent> AddTextStatisticsEventAsync
    (
        TextsStatisticsEventType eventType,
        DateTime eventTime,
        Guid textId,
        int? textPage,
        Guid? creatureId,
        string ip,
        string userAgent
    )
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            throw new ArgumentException("IP must be specified!", nameof(ip));
        }

        if (string.IsNullOrWhiteSpace(userAgent))
        {
            throw new ArgumentException("User agent must be specified!", nameof(userAgent));
        }

        var statisticsEvent = new TextsStatisticsEvent()
        {
            Timestamp = eventTime,
            Text = new Text() { Id = textId },
            Page = textPage,
            Type = eventType,
            CausedByCreature = _creaturesMapper.Map(creatureId.HasValue ? await _userManager.FindByIdAsync(creatureId.Value.ToString()) : null),
            Ip = ip,
            UserAgent = userAgent
        };

        var statisticsEventDbo = _textsStatisticsEventsMapper.Map(statisticsEvent);
        var addedEvent = await _textsStatisticsDao.InsertEventAsync(statisticsEventDbo);

        return _textsStatisticsEventsMapper.Map(addedEvent);
    }

    public async Task<long> GetAllTextsCompleteReadsCountAsync(DateTime startTime, DateTime endTime)
    {
        return await _textsStatisticsDao.GetAllTextsCompleteReadsCountAsync(startTime, endTime);
    }

    public async Task<long> GetAllTextsCompleteReadsCountForLast24HoursAsync()
    {
        var endTime = DateTime.UtcNow;
        var startTime = endTime.AddDays(-1);

        return await GetAllTextsCompleteReadsCountAsync(startTime, endTime);
    }

    public async Task<IReadOnlyCollection<Guid>> GetMostPopularTextsIDsAsync(int skip, int take)
    {
        return await _textsStatisticsDao.GetMostPopularTextsIDsAsync(skip, take);
    }

    public async Task<Dictionary<Guid, long>> GetTextsReadsCountsAsync(IReadOnlyCollection<Guid> textsIds)
    {
        return await _textsStatisticsDao.GetTextsReadsCountAsync(textsIds);
    }

    public async Task<bool> IsTextLikedAsync(Guid textId, Guid creatureId)
    {
        var likeClassEvents = await _textsStatisticsDao.GetEventsCountsAsync
        (
            textId,
            creatureId,
            new[] { TextsStatisticsEventType.Like, TextsStatisticsEventType.UnLike }
        );

        var likesUnlikesDelta = likeClassEvents[TextsStatisticsEventType.Like] - likeClassEvents[TextsStatisticsEventType.UnLike];

        if (likesUnlikesDelta == 0)
        {
            return false;
        }
        else if (likesUnlikesDelta == 1)
        {
            return true;
        }
        else
        {
            throw new InvalidOperationException($"Wrong likes-unlikes delta { likesUnlikesDelta } for text with ID={ textId } and creature with ID={ creatureId }");
        }
    }

    public async Task<bool> IsTextDislikedAsync(Guid textId, Guid creatureId)
    {
        var dislikeClassEvents = await _textsStatisticsDao.GetEventsCountsAsync
        (
            textId,
            creatureId,
            new[] { TextsStatisticsEventType.Dislike, TextsStatisticsEventType.UnDislike }
        );

        var dislikesUndislikesDelta = dislikeClassEvents[TextsStatisticsEventType.Dislike] - dislikeClassEvents[TextsStatisticsEventType.UnDislike];

        if (dislikesUndislikesDelta == 0)
        {
            return false;
        }
        else if (dislikesUndislikesDelta == 1)
        {
            return true;
        }
        else
        {
            throw new InvalidOperationException($"Wrong dislikes-undislikes delta { dislikesUndislikesDelta } for text with ID={ textId } and creature with ID={ creatureId }");
        }
    }

    public async Task<bool> LikeTextAsync
    (
        Guid textId,
        Guid creatureId,
        string ip,
        string userAgent
    )
    {
        if
        (
            await IsTextLikedAsync(textId, creatureId)
            ||
            await IsTextDislikedAsync(textId, creatureId)
        )
        {
            return false;
        }
        
        await AddTextStatisticsEventAsync
        (
            TextsStatisticsEventType.Like,
            DateTime.UtcNow,
            textId,
            null,
            creatureId,
            ip,
            userAgent
        );

        return true;
    }

    public async Task<bool> UnlikeTextAsync
    (
        Guid textId,
        Guid creatureId,
        string ip,
        string userAgent
    )
    {
        if (!await IsTextLikedAsync(textId, creatureId))
        {
            return false;
        }
        
        await AddTextStatisticsEventAsync
        (
            TextsStatisticsEventType.UnLike,
            DateTime.UtcNow,
            textId,
            null,
            creatureId,
            ip,
            userAgent
        );

        return true;
    }

    public async Task<bool> DislikeTextAsync
    (
        Guid textId,
        Guid creatureId,
        string ip,
        string userAgent
    )
    {
        if
        (
            await IsTextLikedAsync(textId, creatureId)
            ||
            await IsTextDislikedAsync(textId, creatureId)
        )
        {
            return false;
        }
        
        await AddTextStatisticsEventAsync
        (
            TextsStatisticsEventType.Dislike,
            DateTime.UtcNow,
            textId,
            null,
            creatureId,
            ip,
            userAgent
        );

        return true;
    }

    public async Task<bool> UndislikeTextAsync
    (
        Guid textId,
        Guid creatureId,
        string ip,
        string userAgent
    )
    {
        if (!await IsTextDislikedAsync(textId, creatureId))
        {
            return false;
        }
        
        await AddTextStatisticsEventAsync
        (
            TextsStatisticsEventType.UnDislike,
            DateTime.UtcNow,
            textId,
            null,
            creatureId,
            ip,
            userAgent
        );

        return true;
    }

    public async Task<long> GetLikesCountAsync(Guid textId)
    {
        var likeUnlikeEvents = await _textsStatisticsDao.GetEventsCountsForAllCreaturesAsync
        (
            textId,
            new[] { TextsStatisticsEventType.Like, TextsStatisticsEventType.UnLike }
        );

        var result = likeUnlikeEvents[TextsStatisticsEventType.Like] - likeUnlikeEvents[TextsStatisticsEventType.UnLike];
        if (result < 0)
        {
            throw new InvalidOperationException($"We have more unlikes than likes for text with ID={ textId }! Likes: { likeUnlikeEvents[TextsStatisticsEventType.Like] }, unlikes: { likeUnlikeEvents[TextsStatisticsEventType.UnLike] }.");
        }

        return result;
    }

    public async Task<long> GetDislikesCountAsync(Guid textId)
    {
        var dislikeUndislikeEvents = await _textsStatisticsDao.GetEventsCountsForAllCreaturesAsync
        (
            textId,
            new[] { TextsStatisticsEventType.Dislike, TextsStatisticsEventType.UnDislike }
        );

        var result = dislikeUndislikeEvents[TextsStatisticsEventType.Dislike] - dislikeUndislikeEvents[TextsStatisticsEventType.UnDislike];
        if (result < 0)
        {
            throw new InvalidOperationException($"We have more undislikes than dislikes for text with ID={ textId }! Dislikes: { dislikeUndislikeEvents[TextsStatisticsEventType.Dislike] }, undislikes: { dislikeUndislikeEvents[TextsStatisticsEventType.UnDislike] }.");
        }

        return result;
    }

    public async Task<bool> IsVotesHistoryVisibleAsync(Guid textId, Guid? creatureId)
    {
        // Uploader, translators and authors will see history, others - not
        if (!creatureId.HasValue)
        {
            return false;
        }

        var textMetadata = await _textsDao.GetTextMetadataByIdAsync(textId);

        if (creatureId.Value == textMetadata.Publisher.Id)
        {
            return true;
        }

        if (textMetadata.Authors.Select(a => a.Id).Contains(creatureId.Value))
        {
            return true;
        }

        if (textMetadata.Translators.Select(t => t.Id).Contains(creatureId.Value))
        {
            return true;
        }

        return false;
    }

    public async Task<IReadOnlyCollection<TextVoteEventDto>> GetVotesEventsAsync(Guid textId, Guid creatureId)
    {
        if (!await IsVotesHistoryVisibleAsync(textId, creatureId))
        {
            throw new InvalidOperationException($"Votes history of text { textId } is unavailable for creature { creatureId }");
        }

        var criticsSettings = await _accountsService.GetCriticsSettingsAsync(creatureId);

        var eventsToGet = new List<TextsStatisticsEventType>() { TextsStatisticsEventType.Like, TextsStatisticsEventType.UnLike };

        if (criticsSettings.IsShowDislikes)
        {
            eventsToGet.Add(TextsStatisticsEventType.Dislike);
            eventsToGet.Add(TextsStatisticsEventType.UnDislike);
        }

        var events = (await _textsStatisticsDao.GetOrderedEventsByTextIdAsync(textId, eventsToGet))
            .OrderByDescending(e => e.Timestamp);

        var votersIds = events
            .Select(e => e.CausedByCreature.Id)
            .Distinct()
            .ToList();

        var voters = await _accountsService.MassGetProfilesByCreaturesIdsAsync(votersIds);

        return events
            .Select
            (
                e => new TextVoteEventDto
                (
                    e.Id,
                    e.Timestamp,
                    e.Type,
                    !criticsSettings.IsShowDislikesAuthors && (e.Type == TextsStatisticsEventType.Dislike || e.Type == TextsStatisticsEventType.UnDislike),
                    voters[e.CausedByCreature.Id].ToDto()
                )
            )
            .ToList();
    }
}