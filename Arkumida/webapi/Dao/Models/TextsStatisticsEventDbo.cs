using System.ComponentModel.DataAnnotations;
using webapi.Dao.Models.Enums.Statistics;

namespace webapi.Dao.Models;

/// <summary>
/// Text statistics event
/// </summary>
public class TextsStatisticsEventDbo
{
    /// <summary>
    /// Event ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// When event occured
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Event is related to this text
    /// </summary>
    public TextDbo Text { get; set; }

    /// <summary>
    /// Event type
    /// </summary>
    public TextsStatisticsEventType Type { get; set; }
    
    /// <summary>
    /// If caused by registered creature, then shi will be here, otherwise here will be null
    /// </summary>
    public CreatureDbo CausedByCreature { get; set; }

    /// <summary>
    /// IPv4 or IPv6 of creature, who caused the event
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// Useragent of creature, who caused the event
    /// </summary>
    public string UserAgent { get; set; }
}