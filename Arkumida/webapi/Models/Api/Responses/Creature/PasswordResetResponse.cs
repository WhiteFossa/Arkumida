using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Password reset confirmation response
/// </summary>
public class PasswordResetResponse
{
    /// <summary>
    /// Is reset successful?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }

    public PasswordResetResponse
    (
        bool isSuccessful
    )
    {
        IsSuccessful = isSuccessful;
    }
}