using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response for "find creature by name" request
/// </summary>
public class FindCreatureByNameResponse
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

    public FindCreatureByNameResponse
    (
        bool isFound,
        CreatureDto creature
    )
    {
        IsFound = isFound;
        Creature = creature; // May be null if not found
    }
}