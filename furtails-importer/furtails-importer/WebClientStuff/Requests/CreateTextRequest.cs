using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

public class CreateTextRequest
{
    /// <summary>
    /// Text
    /// </summary>
    [JsonPropertyName("text")]
    public TextDto Text { get; set; }
}