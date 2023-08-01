using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response for "find creature by login" request
/// </summary>
public class FindCreatureByLoginResponse
{
    /// <summary>
    /// Is creature found?
    /// </summary>
    [JsonPropertyName("isFound")]
    public bool IsFound { get; private set; }

    /// <summary>
    /// Creature (if found)
    /// </summary>
    [JsonPropertyName("creature")]
    public CreatureDto Creature { get; private set; }

    public FindCreatureByLoginResponse
    (
        bool isFound,
        CreatureDto creature
    )
    {
        IsFound = isFound;
        Creature = creature; // May be null if not found
    }
}