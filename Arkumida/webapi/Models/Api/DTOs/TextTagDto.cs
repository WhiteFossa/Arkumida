using System.Text.Json.Serialization;
using webapi.Models.Api.Enums;

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
    /// Tag size category
    /// </summary>
    [JsonPropertyName("sizeCategory")]
    public TagSizeCategory SizeCategory { get; private set; }

    public TextTagDto
    (
        Guid id,
        string furryReadableId,
        string tag,
        TagSizeCategory sizeCategory
    ) : base(id, furryReadableId)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            throw new ArgumentException("Tag must be populated.", nameof(tag));
        }
        Tag = tag;

        SizeCategory = sizeCategory;
    }
}