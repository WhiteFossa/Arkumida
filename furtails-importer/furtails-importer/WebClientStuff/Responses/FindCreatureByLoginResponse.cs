using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class FindCreatureByLoginResponse
{
    /// <summary>
    /// Is creature found?
    /// </summary>
    [JsonPropertyName("isFound")]
    public bool IsFound { get; set; }

    /// <summary>
    /// Creature (if found)
    /// </summary>
    [JsonPropertyName("creature")]
    public CreatureDto Creature { get; set; }
}