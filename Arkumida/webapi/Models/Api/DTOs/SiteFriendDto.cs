using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Information about one site friend
/// </summary>
public class SiteFriendDto
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }
    
    /// <summary>
    /// Friend's name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }
    
    /// <summary>
    /// Friend's resource URL
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; }
    
    /// <summary>
    /// Friend's resource title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }

    public SiteFriendDto
    (
        Guid id,
        string name,
        string url,
        string title
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Friend name mustn't be empty.", nameof(name));
        }
        
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Friend URL mustn't be empty.", nameof(url));
        }
        
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Friend title mustn't be empty.", nameof(title));
        }

        Id = id;
        Name = name;
        Url = url;
        Title = title;
    }
}