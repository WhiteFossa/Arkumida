using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

public class CheckIfEmailTakenRequest
{
    /// <summary>
    /// Check if email taken data
    /// </summary>
    [JsonPropertyName("checkData")]
    public CheckIfEmailTakenDto CheckData { get; set; }
}