using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Right menu response
/// </summary>
public class RightMenuResponse
{
    /// <summary>
    /// Menu items
    /// </summary>
    [JsonPropertyName("items")]
    public IReadOnlyCollection<ImagedLinkDto> Items { get; private set; }

    public RightMenuResponse
    (
        IReadOnlyCollection<ImagedLinkDto> items
    )
    {
        Items = items ?? throw new ArgumentNullException(nameof(items), "Menu items mustn't be null.");
    }
}