using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Tag creation response
/// </summary>
public class CreateTagResponse
{
    /// <summary>
    /// Tag
    /// </summary>
    [JsonPropertyName("tag")]
    public TagDto Tag { get; private set; }

    public CreateTagResponse
    (
        TagDto tag
    )
    {
        Tag = tag ?? throw new ArgumentNullException(nameof(tag), "Tag must be populated.");
    }
}