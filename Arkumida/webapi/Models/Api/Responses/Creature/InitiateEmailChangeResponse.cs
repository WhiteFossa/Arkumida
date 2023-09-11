using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Result of email change initiation
/// </summary>
public class InitiateEmailChangeResponse
{
    /// <summary>
    /// Is confirmation initiated successfully?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }

    /// <summary>
    /// Is confirmation email sent (we wouldn't sent it in case of change to an empty email)?
    /// </summary>
    [JsonPropertyName("isEmailSent")]
    public bool IsEmailSent { get; private set; }

    public InitiateEmailChangeResponse
    (
        bool isSuccessful,
        bool isEmailSent
    )
    {
        IsSuccessful = isSuccessful;
        IsEmailSent = isEmailSent;
    }
}