using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Creature with profile DTO
/// </summary>
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
    
    public CreatureWithProfileDto
    (
        Guid id,
        string furryReadableId,
        string login,
        string email,
        string displayName,
        IReadOnlyCollection<AvatarDto> avatars,
        AvatarDto currentAvatar,
        string about
    ) : base(id, furryReadableId, login, email)
    {
        // All fields may be null (during the text creation, for example)
        DisplayName = displayName;
        Avatars = avatars;
        CurrentAvatar = currentAvatar;
        About = about;
    }

    public CreatureWithProfile ToCreatureWithProfile()
    {
        return new CreatureWithProfile
        (
            Id,
            Login,
            Email,
            false,
            string.Empty,
            DisplayName,
            Avatars?.Select(a => a.ToModel()).ToList(),
            CurrentAvatar?.ToModel(),
            About
        );
    }
}