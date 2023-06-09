using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to create new text section
/// </summary>
public class CreateTextSectionRequest
{
    /// <summary>
    /// Section
    /// </summary>
    [JsonPropertyName("section")]
    public TextSectionDto Section { get; set; }
}