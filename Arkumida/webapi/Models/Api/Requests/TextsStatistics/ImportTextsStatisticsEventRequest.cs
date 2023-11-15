using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs.TextsStatistics;

namespace webapi.Models.Api.Requests.TextsStatistics;

/// <summary>
/// Request to import text statistics event (it can be used only from importer)
/// </summary>
public class ImportTextsStatisticsEventRequest
{
    /// <summary>
    /// Event to import
    /// </summary>
    [JsonPropertyName("textsStatisticsEvent")]
    public TextsStatisticsEventDto TextsStatisticsEvent { get; set; }
}