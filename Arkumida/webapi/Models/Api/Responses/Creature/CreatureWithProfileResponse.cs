using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Response for creature with profile
/// </summary>
public class CreatureWithProfileResponse
{
    /// <summary>
    /// Creature's profile
    /// </summary>
    [JsonPropertyName("creatureWithProfile")]
    public CreatureWithProfileDto CreatureWithProfile { get; private set; }

    public CreatureWithProfileResponse
    (
        CreatureWithProfileDto creatureWithProfile
    )
    {
        CreatureWithProfile = creatureWithProfile ?? throw new ArgumentNullException(nameof(creatureWithProfile), "Profile must be provided!");
    }
}