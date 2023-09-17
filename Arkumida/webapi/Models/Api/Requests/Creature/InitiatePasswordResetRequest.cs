using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Request to initiate password reset process
/// </summary>
public class InitiatePasswordResetRequest
{
    /// <summary>
    /// We will reset password for this creature
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; set; }
}