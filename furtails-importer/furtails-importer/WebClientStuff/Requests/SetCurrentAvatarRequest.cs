using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Requests;

public class SetCurrentAvatarRequest
{
    /// <summary>
    /// Avatar ID
    /// </summary>
    [JsonPropertyName("avatarId")]
    public Guid AvatarId { get; set; }
}