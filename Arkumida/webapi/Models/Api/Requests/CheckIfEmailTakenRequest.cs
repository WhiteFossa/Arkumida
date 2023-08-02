using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to check if email taken
/// </summary>
public class CheckIfEmailTakenRequest
{
    /// <summary>
    /// Check if email taken data
    /// </summary>
    [JsonPropertyName("checkData")]
    public CheckIfEmailTakenDto CheckData { get; set; }
}