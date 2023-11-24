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
        DateTime eventTime,
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
    
    /// <summary>
    /// Get reads count for given texts. If text was never read it will return 0, i.e. item wouldn't be absent
    /// </summary>
    Task<Dictionary<Guid, long>> GetTextsReadsCountsAsync(IReadOnlyCollection<Guid> textsIds);

    #region Likes and dislikes
    
    /// <summary>
    /// Returns true if text liked by given creature (likes - unlikes == 1)
    /// </summary>
    Task<bool> IsTextLikedAsync(Guid textId, Guid creatureId);

    /// <summary>
    /// Returns true if text disliked by given creature (dislikes - undislikes == 1)
    /// </summary>
    Task<bool> IsTextDislikedAsync(Guid textId, Guid creatureId);

    /// <summary>
    /// Likes given text. Returns true in case of success
    /// </summary>
    Task<bool> LikeTextAsync
    (
        Guid textId,
        Guid creatureId,
        string ip,
        string userAgent
    );
    
    /// <summary>
    /// Unlikes given text. Returns true in case of success
    /// </summary>
    Task<bool> UnlikeTextAsync
    (
        Guid textId,
        Guid creatureId,
        string ip,
        string userAgent
    );
    
    /// <summary>
    /// Dislikes given text. Returns true in case of success
    /// </summary>
    Task<bool> DislikeTextAsync
    (
        Guid textId,
        Guid creatureId,
        string ip,
        string userAgent
    );

    /// <summary>
    /// Undislikes given text. Returns true in case of success
    /// </summary>
    Task<bool> UndislikeTextAsync
    (
        Guid textId,
        Guid creatureId,
        string ip,
        string userAgent
    );

    /// <summary>
    /// Get likes count (i.e. likes-unlikes) for given text.
    /// Do not check likes-unlikes balance by users, because furtails-importer will import many likes from one user (importer-user)
    /// </summary>
    Task<long> GetLikesCountAsync(Guid textId);

    /// <summary>
    /// Get dislikes count (i.e. dislikes-undislikes) for given text.
    /// Do not check dislikes-undislikes balance by users, because furtails-importer will import many dislikes from one user (importer-user)
    /// </summary>
    Task<long> GetDislikesCountAsync(Guid textId);

    #endregion
}