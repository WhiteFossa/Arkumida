using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Result of "is login taken" check
/// </summary>
public class CheckIfLoginTakenResultDto
{
    /// <summary>
    /// Is login taken?
    /// </summary>
    [JsonPropertyName("isTaken")]
    public bool IsTaken { get; private set; }

    public CheckIfLoginTakenResultDto
    (
        bool isTaken
    )
    {
        IsTaken = isTaken;
    }
}