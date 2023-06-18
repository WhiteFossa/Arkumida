using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class TextSectionVariantDto
{
    /// <summary>
    /// Variant ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Variant content
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    /// <summary>
    /// Variant creation time
    /// </summary>
    [JsonPropertyName("creationTime")]
    public DateTime CreationTime { get; set; }
}