using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class CheckIfEmailTakenResultDto
{
    /// <summary>
    /// Is email taken?
    /// </summary>
    [JsonPropertyName("isTaken")]
    public bool IsTaken { get; set; }
}