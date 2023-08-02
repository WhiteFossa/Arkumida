using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

public class FindCreatureByLoginRequest
{
    /// <summary>
    /// Search data
    /// </summary>
    [JsonPropertyName("searchData")]
    public FindCreatureByLoginDto SearchData { get; set; }
}