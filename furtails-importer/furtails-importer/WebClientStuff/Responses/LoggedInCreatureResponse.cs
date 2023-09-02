using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class LoggedInCreatureResponse
{
    /// <summary>
    /// Current user
    /// </summary>
    [JsonPropertyName("creature")]
    public CreatureDto Creature { get; set; }
}