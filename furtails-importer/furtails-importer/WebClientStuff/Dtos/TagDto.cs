using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Enums;

namespace furtails_importer.WebClientStuff.Dtos;

public class TagDto
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
    /// Tag subtype
    /// </summary>
    [JsonPropertyName("subtype")]
    public TagSubtype Subtype { get; set; }
    
    /// <summary>
    /// Tag name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// If true, than this tag represents a category
    /// </summary>
    [JsonPropertyName("isCategory")]
    public bool IsCategory { get; set; }

    /// <summary>
    /// If tag is category, then order it by this value
    /// </summary>
    [JsonPropertyName("categoryOrder")]
    public int CategoryOrder { get; set; }
    
    /// <summary>
    /// Category tag type - special types for category tags
    /// </summary>
    [JsonPropertyName("categoryTagType")]
    public CategoryTagType CategoryTagType { get; set; }
    
    /// <summary>
    /// If true, then tag is hidden (see TagDbo.cs for details)
    /// </summary>
    [JsonPropertyName("isHidden")]
    public bool IsHidden { get; set; }
    
    /// <summary>
    /// Machine-readable tag meaning
    /// </summary>
    [JsonPropertyName("meaning")]
    public TagMeaning Meaning { get; set; }
}