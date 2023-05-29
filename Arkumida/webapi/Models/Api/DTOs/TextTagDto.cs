using System.Text.Json.Serialization;

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

    public TextTagDto
    (
        Guid id,
        string humanReadableId,
        string tag
    ) : base(id, humanReadableId)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            throw new ArgumentException("Tag must be populated.", nameof(tag));
        }

        Tag = tag;
    }
}