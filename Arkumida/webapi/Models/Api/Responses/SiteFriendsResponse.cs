using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with all site friends
/// </summary>
public class SiteFriendsResponse
{
    /// <summary>
    /// Friends list
    /// </summary>
    [JsonPropertyName("friends")]
    public IReadOnlyCollection<SiteFriendDto> Friends { get; }

    public SiteFriendsResponse
    (
        IReadOnlyCollection<SiteFriendDto> friends
    )
    {
        Friends = friends ?? throw new ArgumentNullException(nameof(friends), "Friends must be defined");
    }
}