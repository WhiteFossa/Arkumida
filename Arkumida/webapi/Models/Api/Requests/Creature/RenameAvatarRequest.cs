using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Request to rename user's avatar
/// </summary>
public class RenameAvatarRequest
{
    /// <summary>
    /// Avatar ID
    /// </summary>
    [JsonPropertyName("avatarId")]
    public Guid AvatarId { get; set; }
    
    /// <summary>
    /// Avatar new name
    /// </summary>
    [JsonPropertyName("newName")]
    public string NewName { get; set; }
}