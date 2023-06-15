using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class CreateTagResponse
{
    /// <summary>
    /// Tag
    /// </summary>
    [JsonPropertyName("tag")]
    public TagDto Tag { get; set; }
}