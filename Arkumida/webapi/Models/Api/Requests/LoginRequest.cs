using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request for log-in
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Login data
    /// </summary>
    [JsonPropertyName("loginData")]
    public LoginDto LoginData { get; set; }
}