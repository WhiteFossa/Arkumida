using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Link with image
/// </summary>
public class ImagedLinkDto : LinkDto
{
    /// <summary>
    /// Image URL
    /// </summary>
    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; }
    
    /// <summary>
    /// Image alt
    /// </summary>
    [JsonPropertyName("imageAlt")]
    public string ImageAlt { get; }
    
    /// <summary>
    /// Image class
    /// </summary>
    [JsonPropertyName("imageClass")]
    public string ImageClass { get; }
    
    public ImagedLinkDto(string url, string text, string title, string imageUrl, string imageAlt, string imageClass) : base(url, text, title)
    {
        // We dont check for Image URL, ImageAlt and ImageClass emptiness because it can be empty (if link have no image for example)
        ImageUrl = imageUrl;
        ImageAlt = imageAlt;
        ImageClass = imageClass;
    }
}