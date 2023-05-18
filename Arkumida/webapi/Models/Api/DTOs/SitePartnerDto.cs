using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Site partners banners
/// </summary>
public class SitePartnerDto
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }
    
    /// <summary>
    /// Partner URL
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; }

    /// <summary>
    /// Link title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }
    
    /// <summary>
    /// Banner URL (relative, mostly probably)
    /// </summary>
    [JsonPropertyName("bannerUrl")]
    public string BannerUrl { get; }

    /// <summary>
    /// Alternative text for banner image
    /// </summary>
    [JsonPropertyName("bannerAlt")]
    public string BannerAlt { get; }

    public SitePartnerDto
    (
        Guid id,
        string url,
        string title,
        string bannerUrl,
        string bannerAlt
    )
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Partner URL mustn't be empty.", nameof(url));
        }
        
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Partner link title mustn't be empty.", nameof(title));
        }
        
        if (string.IsNullOrWhiteSpace(bannerUrl))
        {
            throw new ArgumentException("Partner banner URL mustn't be empty.", nameof(bannerUrl));
        }
        
        if (string.IsNullOrWhiteSpace(bannerAlt))
        {
            throw new ArgumentException("Partner banner alternative text mustn't be empty.", nameof(bannerAlt));
        }

        Id = id;
        Url = url;
        Title = title;
        BannerUrl = bannerUrl;
        BannerAlt = bannerAlt;
    }
}