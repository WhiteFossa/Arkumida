using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class CheckIfLoginTakenResponse
{
    /// <summary>
    /// Check result
    /// </summary>
    [JsonPropertyName("checkResult")]
    public CheckIfLoginTakenResultDto CheckResult { get; set; }
}