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
    
    public CreatureWithProfileDto
    (
        Guid id,
        string furryReadableId,
        string login,
        string email,
        string displayName,
        IReadOnlyCollection<AvatarDto> avatars,
        AvatarDto currentAvatar
    ) : base(id, furryReadableId, login, email)
    {
        // All fields may be null (during the text creation, for example)
        DisplayName = displayName;
        Avatars = avatars;
        CurrentAvatar = currentAvatar;
    }

    public CreatureWithProfile ToCreatureWithProfile()
    {
        return new CreatureWithProfile
        (
            Id,
            Login,
            Email,
            DisplayName,
            Avatars?.Select(a => a.ToModel()).ToList(),
            CurrentAvatar?.ToModel()
        );
    }
}