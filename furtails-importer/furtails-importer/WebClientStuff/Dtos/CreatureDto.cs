using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

/// <summary>
/// User
/// </summary>
public class CreatureDto : IdedEntityDto
{
    /// <summary>
    /// Creature login
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; set; }

    /// <summary>
    /// Creature email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
}