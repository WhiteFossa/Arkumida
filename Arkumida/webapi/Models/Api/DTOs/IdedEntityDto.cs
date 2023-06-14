using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Entity with Id
/// </summary>
public class IdedEntityDto
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

    public IdedEntityDto
    (
        Guid id,
        string furryReadableId
    )
    {
        if (string.IsNullOrWhiteSpace(furryReadableId))
        {
            throw new ArgumentException("Furry readable ID must be populated.", nameof(furryReadableId));
        }

        Id = id;
        FurryReadableId = furryReadableId;
    }
}