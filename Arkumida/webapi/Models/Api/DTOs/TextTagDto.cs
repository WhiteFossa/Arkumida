using System.Text.Json.Serialization;
using webapi.Dao.Models.Enums;
using webapi.Models.Enums;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Tag for text
/// </summary>
public class TextTagDto : IdedEntityDto
{
    /// <summary>
    /// Tag title
    /// </summary>
    [JsonPropertyName("tag")]
    public string Tag { get; private set; }

    /// <summary>
    /// If true, then display it before hash
    /// </summary>
    [JsonPropertyName("isCategory")]
    public bool IsCategory { get; private set; }
    
    /// <summary>
    /// Tag size category
    /// </summary>
    [JsonPropertyName("sizeCategory")]
    public TagSizeCategory SizeCategory { get; private set; }

    /// <summary>
    /// Tag meaning
    /// </summary>
    [JsonPropertyName("meaning")]
    public TagMeaning Meaning { get; private set; }

    public TextTagDto
    (
        Guid id,
        string furryReadableId,
        string tag,
        bool isCategory,
        TagSizeCategory sizeCategory,
        TagMeaning meaning
    ) : base(id, furryReadableId)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            throw new ArgumentException("Tag must be populated.", nameof(tag));
        }
        Tag = tag;
        IsCategory = isCategory;
        SizeCategory = sizeCategory;
        Meaning = meaning;
    }
}