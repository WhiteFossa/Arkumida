using System.Text.Json.Serialization;
using webapi.Models.Enums;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// DTO with user registration result
/// </summary>
public class RegistrationResultDto
{
    /// <summary>
    /// Registered user ID
    /// </summary>
    [JsonPropertyName("userId")]
    public string UserId { get; private set; }

    /// <summary>
    /// Registration result
    /// </summary>
    [JsonPropertyName("result")]
    public UserRegistrationResult Result { get; private set; }

    public RegistrationResultDto
    (
        string userId,
        UserRegistrationResult result
    )
    {
        UserId = userId; // Might be empty in case of error
        Result = result;
    }
}