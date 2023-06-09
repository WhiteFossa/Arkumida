using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with one category tag
/// </summary>
public class CategoryTagResponse
{
    /// <summary>
    /// Category tag
    /// </summary>
    [JsonPropertyName("categoryTag")]
    public CategoryTagDto CategoryTag { get; private set; }

    public CategoryTagResponse
    (
        CategoryTagDto categoryTag
    )
    {
        CategoryTag = categoryTag ?? throw new ArgumentNullException(nameof(categoryTag), "Category tag must not be null.");
    }
}