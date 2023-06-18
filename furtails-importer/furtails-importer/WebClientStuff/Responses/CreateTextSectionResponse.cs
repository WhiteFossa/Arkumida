using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class CreateTextSectionResponse
{
    /// <summary>
    /// Section
    /// </summary>
    [JsonPropertyName("section")]
    public TextSectionDto Section { get; set; }
}