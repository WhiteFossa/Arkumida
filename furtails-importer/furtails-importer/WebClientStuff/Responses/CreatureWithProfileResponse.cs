using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class CreatureWithProfileResponse
{
    /// <summary>
    /// Creature's profile
    /// </summary>
    [JsonPropertyName("creatureWithProfile")]
    public CreatureWithProfileDto CreatureWithProfile { get; set; }
}