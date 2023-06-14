using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to create a new tag
/// </summary>
public class CreateTagRequest
{
    /// <summary>
    /// Tag
    /// </summary>
    [JsonPropertyName("tag")]
    public TagDto Tag { get; set; }
}