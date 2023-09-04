using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Request to delete avatar
/// </summary>
public class DeleteAvatarRequest
{
    /// <summary>
    /// Avatar ID
    /// </summary>
    [JsonPropertyName("avatarId")]
    public Guid AvatarId { get; set; }
}