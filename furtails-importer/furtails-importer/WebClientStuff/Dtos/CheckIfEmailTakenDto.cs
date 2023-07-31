using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class CheckIfEmailTakenDto
{
    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
}