using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Result for check "Is email taken?"
/// </summary>
public class CheckIfEmailTakenResultDto
{
    /// <summary>
    /// Is email taken?
    /// </summary>
    [JsonPropertyName("isTaken")]
    public bool IsTaken { get; private set; }

    public CheckIfEmailTakenResultDto
    (
        bool isTaken
    )
    {
        IsTaken = isTaken;
    }
}