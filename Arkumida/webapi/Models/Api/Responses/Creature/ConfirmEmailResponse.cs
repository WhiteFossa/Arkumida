using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Response with email confirmation result
/// </summary>
public class ConfirmEmailResponse
{
    /// <summary>
    /// Is confirmation initiated successfully?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }

    public ConfirmEmailResponse
    (
        bool isSuccessful
    )
    {
        IsSuccessful = isSuccessful;
    }
}