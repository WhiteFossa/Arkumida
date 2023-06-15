using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

/// <summary>
/// Request to create a new tag
/// </summary>
public class CreateTagRequest
{
    /// <summary>
    /// Tag
    /// </summary>
    [JsonPropertyName("tag")]
    public TagDto Tag { get; set; }
}