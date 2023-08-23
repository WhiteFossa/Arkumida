using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with user's current avatar
/// </summary>
public class CurrentAvatarResponse
{
    /// <summary>
    /// If true, then avatar is set, otherwise the creature is avatarless
    /// </summary>
    [JsonPropertyName("isSet")]
    public bool IsSet { get; private set; }

    /// <summary>
    /// Creature's current avatar (may be null)
    /// </summary>
    [JsonPropertyName("currentAvatar")]
    public AvatarDto CurrentAvatar { get; private set; }

    public CurrentAvatarResponse
    (
        bool isSet,
        AvatarDto currentAvatar
    )
    {
        IsSet = isSet;
        CurrentAvatar = currentAvatar;
    }
}