using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Author
/// </summary>
public class CreatureDto : IdedEntityDto
{
    /// <summary>
    /// Creature name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; private set; }

    public CreatureDto
    (
        Guid id,
        string furryReadableId,
        string name
    ) : base(id, furryReadableId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be populated", nameof(name));
        }

        Name = name;
    }
}