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
    /// Stories read today
    /// </summary>
    [JsonPropertyName("readToday")]
    public int ReadToday { get; private set; }

    /// <summary>
    /// Last story was added this time
    /// </summary>
    [JsonPropertyName("lastAddTime")]
    public DateTime LastAddTime { get; private set; }

    public TextsStatisticsResponse
    (
        int totalTexts,
        int readToday,
        DateTime lastAddTime
    )
    {
        if (totalTexts < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalTexts), "Total texts can't be negative.");
        }
        
        if (readToday < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(readToday), "Amount of texts read today can't be negative.");
        }

        TotalTexts = totalTexts;
        ReadToday = readToday;
        LastAddTime = lastAddTime;
    }
}