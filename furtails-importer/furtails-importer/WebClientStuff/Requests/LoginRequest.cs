using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

public class LoginRequest
{
    /// <summary>
    /// Login data
    /// </summary>
    [JsonPropertyName("loginData")]
    public LoginDto LoginData { get; set; }
}