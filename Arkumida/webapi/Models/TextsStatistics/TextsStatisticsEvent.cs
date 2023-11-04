using webapi.Dao.Models.Enums.Statistics;

namespace webapi.Models.TextsStatistics;

/// <summary>
/// Texts statistics event model
/// </summary>
public class TextsStatisticsEvent
{
    /// <summary>
    /// Event ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// When event occured
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Event is related to this text
    /// </summary>
    public Text Text { get; set; }
    
    /// <summary>
    /// Event is related to this text page
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Event type
    /// </summary>
    public TextsStatisticsEventType Type { get; set; }
    
    /// <summary>
    /// If caused by registered creature, then shi will be here, otherwise here will be null
    /// </summary>
    public Creature CausedByCreature { get; set; }

    /// <summary>
    /// IPv4 or IPv6 of creature, who caused the event
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// Useragent of creature, who caused the event
    /// </summary>
    public string UserAgent { get; set; }
}