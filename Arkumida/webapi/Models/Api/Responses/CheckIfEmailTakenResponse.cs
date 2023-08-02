using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Result for "is email taken?" check
/// </summary>
public class CheckIfEmailTakenResponse
{
    /// <summary>
    /// Check result
    /// </summary>
    [JsonPropertyName("checkResult")]
    public CheckIfEmailTakenResultDto CheckResult { get; private set; }

    public CheckIfEmailTakenResponse
    (
        CheckIfEmailTakenResultDto checkResult
    )
    {
        CheckResult = checkResult ?? throw new ArgumentNullException(nameof(checkResult), "Check result must not be null!");
    }
}