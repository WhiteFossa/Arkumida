using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Request to change email (aka "confirm change")
/// </summary>
public class ChangeEmailRequest
{
    /// <summary>
    /// Email (as BASE64)
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    /// <summary>
    /// Change token
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; }
}