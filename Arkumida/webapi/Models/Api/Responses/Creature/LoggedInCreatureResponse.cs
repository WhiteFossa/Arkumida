using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Current (logged in) user
/// </summary>
public class LoggedInCreatureResponse
{
    /// <summary>
    /// Current user
    /// </summary>
    [JsonPropertyName("creature")]
    public CreatureDto Creature { get; private set; }
    
    public LoggedInCreatureResponse(CreatureDto creature)
    {
        Creature = creature ?? throw new ArgumentNullException(nameof(creature), "Creature must not be null!");
    }
}