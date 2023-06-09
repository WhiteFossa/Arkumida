using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with categories tags
/// </summary>
public class CategoriesTagsListResponse
{
    /// <summary>
    /// Categories tags
    /// </summary>
    [JsonPropertyName("categoriesTags")]
    public IReadOnlyCollection<CategoryTagDto> CategoriesTags { get; private set; }

    public CategoriesTagsListResponse
    (
        IReadOnlyCollection<CategoryTagDto> categoriesTags
    )
    {
        CategoriesTags = categoriesTags ?? throw new ArgumentNullException(nameof(categoriesTags), "Category tags list can't be null.");
    }
}