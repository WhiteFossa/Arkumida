using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

public class CreateTextSectionRequest
{
    /// <summary>
    /// Section
    /// </summary>
    [JsonPropertyName("section")]
    public TextSectionDto Section { get; set; }
}