using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class LoginResponse
{
    /// <summary>
    /// Login result
    /// </summary>
    [JsonPropertyName("loginResult")]
    public LoginResultDto LoginResult { get; set; }
}