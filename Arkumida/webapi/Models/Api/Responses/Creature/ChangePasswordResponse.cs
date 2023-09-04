using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Response with information about changed password
/// </summary>
public class ChangePasswordResponse
{
    /// <summary>
    /// Did password changed successfully?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; private set; }

    public ChangePasswordResponse
    (
        bool isSuccessful
    )
    {
        IsSuccessful = isSuccessful;
    }
}