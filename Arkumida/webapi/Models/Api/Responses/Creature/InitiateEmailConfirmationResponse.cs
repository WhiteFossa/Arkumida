using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Response for email confirmation initiation
/// </summary>
public class InitiateEmailConfirmationResponse
{
    /// <summary>
    /// Is confirmation initiated successfully?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }

    public InitiateEmailConfirmationResponse
    (
        bool isSuccessful
    )
    {
        IsSuccessful = isSuccessful;
    }
}