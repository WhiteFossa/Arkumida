using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to change creature's about information
/// </summary>
public class UpdateAboutInfoRequest
{
    /// <summary>
    /// New about info
    /// </summary>
    [JsonPropertyName("newAbout")]
    public string NewAbout { get; set; }
}