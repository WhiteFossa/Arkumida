using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

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