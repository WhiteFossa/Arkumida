using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Request to initiate creature's email change
/// </summary>
public class InitiateEmailChangeRequest
{
    /// <summary>
    /// New email
    /// </summary>
    [JsonPropertyName("newEmail")]
    public string NewEmail { get; set; }
}