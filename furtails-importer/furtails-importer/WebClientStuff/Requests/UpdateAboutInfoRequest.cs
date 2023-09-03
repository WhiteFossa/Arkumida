using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Requests;

public class UpdateAboutInfoRequest
{
    /// <summary>
    /// New about info
    /// </summary>
    [JsonPropertyName("newAbout")]
    public string NewAbout { get; set; }
}