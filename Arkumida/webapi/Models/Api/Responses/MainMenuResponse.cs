using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Main menu items
/// </summary>
public class MainMenuResponse
{
    /// <summary>
    /// Menu items
    /// </summary>
    [JsonPropertyName("items")]
    public IReadOnlyCollection<LinkDto> Items { get; private set; }

    public MainMenuResponse
    (
        IReadOnlyCollection<LinkDto> items
    )
    {
        Items = items ?? throw new ArgumentNullException(nameof(items), "Menu items mustn't be null.");
    }
}