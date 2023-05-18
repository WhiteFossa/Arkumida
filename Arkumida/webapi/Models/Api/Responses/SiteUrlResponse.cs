using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with site URL
/// </summary>
public class SiteUrlResponse
{
    /// <summary>
    /// Site URL
    /// </summary>
    [JsonPropertyName("siteUrl")]
    public string SiteUrl { get; }
    
    /// <summary>
    /// Site title
    /// </summary>
    [JsonPropertyName("siteTitle")]
    public string SiteTitle { get; }

    public SiteUrlResponse
    (
        string siteUrl,
        string siteTitle
    )
    {
        if (string.IsNullOrWhiteSpace(siteUrl))
        {
            throw new ArgumentException("Site URL must not be empty.", nameof(siteUrl));
        }
        
        if (string.IsNullOrWhiteSpace(siteTitle))
        {
            throw new ArgumentException("Site title must not be empty.", nameof(siteTitle));
        }

        SiteUrl = siteUrl;
        SiteTitle = siteTitle;
    }
}