using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Email change response
/// </summary>
public class ChangeEmailResponse
{
    /// <summary>
    /// Is change successful?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }

    public ChangeEmailResponse
    (
        bool isSuccessful
    )
    {
        IsSuccessful = isSuccessful;
    }
}