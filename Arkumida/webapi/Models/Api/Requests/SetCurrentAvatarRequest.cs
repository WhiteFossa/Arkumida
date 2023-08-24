using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Set current avatar for current user
/// </summary>
public class SetCurrentAvatarRequest
{
    /// <summary>
    /// Avatar ID
    /// </summary>
    [JsonPropertyName("avatarId")]
    public Guid AvatarId { get; set; }
}