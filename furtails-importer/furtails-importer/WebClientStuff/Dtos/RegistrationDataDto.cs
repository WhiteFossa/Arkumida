using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class RegistrationDataDto
{
    [JsonPropertyName("login")]
    public string Login { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("password")]
    public string Password { get; set; }
}