using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

/// <summary>
/// User
/// </summary>
public class CreatureDto : IdedEntityDto
{
    /// <summary>
    /// Creature name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get;  set; }
}