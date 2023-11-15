using System.Text.Json.Serialization;
using webapi.Dao.Models.Enums.Statistics;
using webapi.Models.TextsStatistics;

namespace webapi.Models.Api.DTOs.TextsStatistics;

/// <summary>
/// Texts statistics event DTO
/// </summary>
public class TextsStatisticsEventDto
{
    /// <summary>
    /// Event ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// When event occured
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Event is related to this text
    /// </summary>
    [JsonPropertyName("textId")]
    public Guid TextId { get; set; }
    
    /// <summary>
    /// Event is related to this text page
    /// </summary>
    [JsonPropertyName("page")]
    public int? Page { get; set; }

    /// <summary>
    /// Event type
    /// </summary>
    [JsonPropertyName("type")]
    public TextsStatisticsEventType Type { get; set; }
    
    /// <summary>
    /// If caused by registered creature, then shi will be here, otherwise here will be null
    /// </summary>
    [JsonPropertyName("creatureId")]
    public Guid? CreatureId { get; set; }

    /// <summary>
    /// IPv4 or IPv6 of creature, who caused the event
    /// </summary>
    [JsonPropertyName("ip")]
    public string Ip { get; set; }

    /// <summary>
    /// Useragent of creature, who caused the event
    /// </summary>
    [JsonPropertyName("userAgent")]
    public string UserAgent { get; set; }
    
    public TextsStatisticsEvent ToModel()
    {
        return new TextsStatisticsEvent()
        {
            Id = Id,
            Timestamp = Timestamp,
            Text = new Text() { Id = TextId },
            Page = Page,
            Type = Type,
            CausedByCreature = CreatureId.HasValue ? new Creature(Id, String.Empty, String.Empty) : null,
            Ip = Ip,
            UserAgent = UserAgent
        };
    }
}