using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to find a creature by login
/// </summary>
public class FindCreatureByLoginRequest
{
    /// <summary>
    /// Search data
    /// </summary>
    [JsonPropertyName("searchData")]
    public FindCreatureByLoginDto SearchData { get; set; }
}