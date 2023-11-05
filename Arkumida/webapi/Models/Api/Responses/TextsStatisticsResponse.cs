using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Information about stories on site
/// </summary>
public class TextsStatisticsResponse
{
    /// <summary>
    /// Total texts
    /// </summary>
    [JsonPropertyName("totalTexts")]
    public int TotalTexts { get; private set; }
    
    /// <summary>
    /// Stories read for last 24 hours
    /// </summary>
    [JsonPropertyName("readDuringLast24Hours")]
    public long ReadDuringLast24Hours { get; private set; }

    /// <summary>
    /// Last story was added this time
    /// </summary>
    [JsonPropertyName("lastAddTime")]
    public DateTime LastAddTime { get; private set; }

    public TextsStatisticsResponse
    (
        int totalTexts,
        long readDuringLast24Hours,
        DateTime lastAddTime
    )
    {
        if (totalTexts < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalTexts), "Total texts can't be negative.");
        }
        
        if (readDuringLast24Hours < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(readDuringLast24Hours), "Amount of texts read during last 24 hours can't be negative.");
        }

        TotalTexts = totalTexts;
        ReadDuringLast24Hours = readDuringLast24Hours;
        LastAddTime = lastAddTime;
    }
}