using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses.Creature;

/// <summary>
/// Information about is email confirmed or not
/// </summary>
public class IsEmailConfirmedResponse
{
    /// <summary>
    /// Is email confirmed?
    /// </summary>
    [JsonPropertyName("isConfirmed")]
    public bool IsConfirmed { get; private set; }

    public IsEmailConfirmedResponse
    (
        bool isConfirmed
    )
    {
        IsConfirmed = isConfirmed;
    }
}