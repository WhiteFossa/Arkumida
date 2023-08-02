using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// User registration response
/// </summary>
public class UserRegistrationResponse
{
    [JsonPropertyName("registrationResult")]
    public RegistrationResultDto RegistrationResult { get; private set; }

    public UserRegistrationResponse
    (
        RegistrationResultDto registrationResult
    )
    {
        RegistrationResult = registrationResult ?? throw new ArgumentNullException(nameof(registrationResult), "Registration result must not be null");
    }
}