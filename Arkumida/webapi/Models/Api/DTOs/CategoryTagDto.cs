using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Tag for category
/// </summary>
public class CategoryTagDto : IdedEntityDto
{
    /// <summary>
    /// Tag title
    /// </summary>
    [JsonPropertyName("tag")]
    public string Tag { get; private set; }

    /// <summary>
    /// How much texts are marked with this tag
    /// </summary>
    [JsonPropertyName("textsCount")]
    public int TextsCount { get; private set; }
    
    public CategoryTagDto
    (
        Guid id,
        string furryReadableId,
        string tag,
        int textsCount
    ) : base(id, furryReadableId)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            throw new ArgumentException("Tag must be populated.", nameof(tag));
        }
        Tag = tag;

        if (textsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(textsCount), "Texts count must be non-negative.");
        }
        TextsCount = textsCount;
    }
}