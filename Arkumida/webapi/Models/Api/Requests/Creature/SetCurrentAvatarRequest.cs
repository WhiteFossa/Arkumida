using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Set current avatar for current user
/// </summary>
public class SetCurrentAvatarRequest
{
    /// <summary>
    /// Avatar ID (may be null if user choosing not to not have an avatar at all)
    /// </summary>
    [JsonPropertyName("avatarId")]
    public Guid? AvatarId { get; set; }
}