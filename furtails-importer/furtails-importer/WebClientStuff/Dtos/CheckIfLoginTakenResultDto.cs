using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class CheckIfLoginTakenResultDto
{
    /// <summary>
    /// Is login taken?
    /// </summary>
    [JsonPropertyName("isTaken")]
    public bool IsTaken { get; set; }
}