using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class TextTagDto
{
    /// <summary>
    /// Entity Id
    /// </summary>
    [JsonPropertyName("entityId")]
    public Guid Id { get; set; }

    /// <summary>
    /// Furry-readable ID (mostly for compatibility with old site)
    /// </summary>
    [JsonPropertyName("furryReadableId")]
    public string FurryReadableId { get; set; }
    
    /// <summary>
    /// Tag title
    /// </summary>
    [JsonPropertyName("tag")]
    public string Tag { get; set; }

    /*/// <summary>
    /// Tag size category
    /// </summary>
    [JsonPropertyName("sizeCategory")]
    public TagSizeCategory SizeCategory { get; set; }*/
}