using System.Text.Json.Serialization;
using webapi.Models.Enums;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Password reset initiation response
/// </summary>
public class PasswordResetInitiationResponse
{
    /// <summary>
    /// Password reset initiation result
    /// </summary>
    [JsonPropertyName("result")]
    public PasswordResetInitiationResult Result { get; private set; }

    public PasswordResetInitiationResponse
    (
        PasswordResetInitiationResult result
    )
    {
        Result = result;
    }
}