using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// DTO for checking if email taken?
/// </summary>
public class CheckIfEmailTakenDto
{
    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
}