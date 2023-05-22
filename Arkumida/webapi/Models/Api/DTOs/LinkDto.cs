using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// DTO, representing a link
/// </summary>
public class LinkDto
{
    /// <summary>
    /// URL
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; }
    
    /// <summary>
    /// Link text
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; }
    
    /// <summary>
    /// Link title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }

    public LinkDto
    (
        string url,
        string text,
        string title
    )
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Link URL mustn't be empty.", nameof(url));
        }
        
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Link text mustn't be empty.", nameof(text));
        }
        
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Link title mustn't be empty.", nameof(title));
        }

        Url = url;
        Text = text;
        Title = title;
    }
}