using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class UserRegistrationResponse
{
    [JsonPropertyName("registrationResult")]
    public RegistrationResultDto RegistrationResult { get; set; }
}