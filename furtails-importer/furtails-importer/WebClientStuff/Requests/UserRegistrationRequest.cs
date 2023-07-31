using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

public class UserRegistrationRequest
{
    /// <summary>
    /// Registration data
    /// </summary>
    [JsonPropertyName("registrationData")]
    public RegistrationDataDto RegistrationData { get; set; }
}