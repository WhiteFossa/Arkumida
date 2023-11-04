namespace webapi.Dao.Models.Enums.Statistics;

/// <summary>
/// Possible event type for statistics log
/// </summary>
public enum TextsStatisticsEventType
{
    /// <summary>
    /// Text page was read
    /// </summary>
    PageRead = 0,
    
    /// <summary>
    /// Text was liked
    /// </summary>
    Like = 1,
    
    /// <summary>
    /// Text was disliked
    /// </summary>
    Dislike = 2,
    
    /// <summary>
    /// Text was completely read by creature
    /// </summary>
    TextReadCompleted = 3
}