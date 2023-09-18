using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Request to reset password
/// </summary>
public class PasswordResetRequest
{
    /// <summary>
    /// New password
    /// </summary>
    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
    
    /// <summary>
    /// Reset token
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; }
}