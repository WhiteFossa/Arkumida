using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

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