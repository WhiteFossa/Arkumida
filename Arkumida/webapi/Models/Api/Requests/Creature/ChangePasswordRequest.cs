using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Request to change creature's password
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// Old password
    /// </summary>
    [JsonPropertyName("oldPassword")]
    public string OldPassword { get; set; }
    
    /// <summary>
    /// New password
    /// </summary>
    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
}