using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

public class CheckIfLoginTakenRequest
{
    /// <summary>
    /// Check if login taken data
    /// </summary>
    [JsonPropertyName("checkData")]
    public CheckIfLoginTakenDto CheckData { get; set; }
}