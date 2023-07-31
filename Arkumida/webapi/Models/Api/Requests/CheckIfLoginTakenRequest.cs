using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to check if login taken
/// </summary>
public class CheckIfLoginTakenRequest
{
    /// <summary>
    /// Check if login taken data
    /// </summary>
    [JsonPropertyName("checkData")]
    public CheckIfLoginTakenDto CheckData { get; set; }
}