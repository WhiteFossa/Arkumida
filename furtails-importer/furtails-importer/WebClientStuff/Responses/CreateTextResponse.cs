using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class CreateTextResponse
{
    /// <summary>
    /// Text
    /// </summary>
    [JsonPropertyName("text")]
    public TextDto Text { get; set; }
}