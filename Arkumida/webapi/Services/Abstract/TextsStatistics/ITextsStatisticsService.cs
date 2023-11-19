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
    /// Likes given text. If it's currently liked or disliked by creature - throws exception
    /// </summary>
    Task LikeTextAsync
    (
        Guid textId,
        Guid creatureId,
        string ip,
        string userAgent
    );

    #endregion
}