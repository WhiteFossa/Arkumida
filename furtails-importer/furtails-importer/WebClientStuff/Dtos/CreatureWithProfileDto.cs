using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class CreatureWithProfileDto : CreatureDto
{
    /// <summary>
    /// User's visible name
    /// </summary>
    [JsonPropertyName("name")]
    public string DisplayName { get; set; }
    
    /// <summary>
    /// Creature's avatars
    /// </summary>
    [JsonPropertyName("avatars")]
    public IReadOnlyCollection<AvatarDto> Avatars { get; set; }

    /// <summary>
    /// Creature's current avatar
    /// </summary>
    [JsonPropertyName("currentAvatar")]
    public AvatarDto CurrentAvatar { get; set; }
    
    /// <summary>
    /// Information about creature (in the format, processable by ITextUtilsService.ParseTextToElements())
    /// </summary>
    [JsonPropertyName("about")]
    public string About { get; set; }
}