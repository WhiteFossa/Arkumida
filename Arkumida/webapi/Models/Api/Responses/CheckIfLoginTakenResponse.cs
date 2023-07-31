using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Result for "is login taken?" check
/// </summary>
public class CheckIfLoginTakenResponse
{
    /// <summary>
    /// Check result
    /// </summary>
    [JsonPropertyName("checkResult")]
    public CheckIfLoginTakenResultDto CheckResult { get; private set; }

    public CheckIfLoginTakenResponse
    (
        CheckIfLoginTakenResultDto checkResult
    )
    {
        CheckResult = checkResult ?? throw new ArgumentNullException(nameof(checkResult), "Check result must not be null!");
    }
}