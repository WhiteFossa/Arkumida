using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs.TextsStatistics;

namespace webapi.Models.Api.Responses.TextsStatistics;

/// <summary>
/// Response with imported event
/// </summary>
public class ImportTextsStatisticsEventResponse
{
    /// <summary>
    /// Imported event
    /// </summary>
    [JsonPropertyName("textsStatisticsEvent")]
    public TextsStatisticsEventDto TextsStatisticsEvent { get; set; }
}