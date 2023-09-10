using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Email address confirmation request
/// </summary>
public class ConfirmEmailRequest
{
    /// <summary>
    /// Confirmation token
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; }
}