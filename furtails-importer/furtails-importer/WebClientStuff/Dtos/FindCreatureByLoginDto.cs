using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class FindCreatureByLoginDto
{
    /// <summary>
    /// Login
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; set; }
}