using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to register a new user
/// </summary>
public class UserRegistrationRequest
{
    /// <summary>
    /// Registration data
    /// </summary>
    [JsonPropertyName("registrationData")]
    public RegistrationDataDto RegistrationData { get; set; }
}