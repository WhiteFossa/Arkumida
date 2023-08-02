using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class LoginDto
{
    /// <summary>
    /// Login
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; set; }
    
    /// <summary>
    /// Password
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; }
}