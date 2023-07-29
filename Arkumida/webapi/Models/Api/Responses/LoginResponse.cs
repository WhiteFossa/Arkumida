using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response for login process
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Login result
    /// </summary>
    [JsonPropertyName("loginResult")]
    public LoginResultDto LoginResult { get; private set; }

    public LoginResponse
    (
        LoginResultDto loginResult
    )
    {
        LoginResult = loginResult ?? throw new ArgumentNullException(nameof(loginResult), "Login result must not be null!");
    }
}