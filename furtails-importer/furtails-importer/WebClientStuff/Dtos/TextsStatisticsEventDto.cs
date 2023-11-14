using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Enums;

namespace furtails_importer.WebClientStuff.Dtos;

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
}