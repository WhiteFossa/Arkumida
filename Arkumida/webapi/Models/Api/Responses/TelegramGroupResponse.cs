using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with a link to Telegram group
/// </summary>
public class TelegramGroupResponse
{
    /// <summary>
    /// Group URL
    /// </summary>
    [JsonPropertyName("groupUrl")]
    public string GroupUrl { get; }
    
    /// <summary>
    /// Group title
    /// </summary>
    [JsonPropertyName("groupTitle")]
    public string GroupTitle { get; }

    public TelegramGroupResponse
    (
        string groupUrl,
        string groupTitle
    )
    {
        if (string.IsNullOrWhiteSpace(groupUrl))
        {
            throw new ArgumentException("Group URL must not be empty.", nameof(groupUrl));
        }
        
        if (string.IsNullOrWhiteSpace(groupTitle))
        {
            throw new ArgumentException("Group title must not be empty.", nameof(groupTitle));
        }

        GroupUrl = groupUrl;
        GroupTitle = groupTitle;
    }
}