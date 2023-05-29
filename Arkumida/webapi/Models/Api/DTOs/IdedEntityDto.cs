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
    public Guid Id { get; private set; }

    /// <summary>
    /// Human-readable ID (mostly for compatibility with old site)
    /// </summary>
    [JsonPropertyName("humanReadableId")]
    public string HumanReadableId { get; private set; }

    public IdedEntityDto
    (
        Guid id,
        string humanReadableId
    )
    {
        if (string.IsNullOrWhiteSpace(humanReadableId))
        {
            throw new ArgumentException("Human readable ID must be populated.", nameof(humanReadableId));
        }

        Id = id;
        HumanReadableId = humanReadableId;
    }
}