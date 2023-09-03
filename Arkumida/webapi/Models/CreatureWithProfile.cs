using webapi.Models.Api.DTOs;

namespace webapi.Models;

/// <summary>
/// Creature with profile
/// </summary>
public class CreatureWithProfile : Creature
{
    /// <summary>
    /// User's visible name
    /// </summary>
    public string DisplayName { get; set; }
    
    /// <summary>
    /// Creature's avatars
    /// </summary>
    public IList<Avatar> Avatars { get; set; }

    /// <summary>
    /// Creature's current avatar
    /// </summary>
    public Avatar CurrentAvatar { get; set; }
    
    /// <summary>
    /// Information about creature (in the format, processable by ITextUtilsService.ParseTextToElements())
    /// </summary>
    public string About { get; set; }
    
    public CreatureWithProfile
    (
        Guid id,
        string login,
        string email,
        string displayName,
        IList<Avatar> avatars,
        Avatar currentAvatar,
        string about
    ) : base(id, login, email)
    {
        // All fields may be null (for example when creating a new text)
        DisplayName = displayName;
        Avatars = avatars;
        CurrentAvatar = currentAvatar;
        About = about;
    }

    public new CreatureWithProfileDto ToDto()
    {
        return new CreatureWithProfileDto
        (
            Id,
            "not_ready",
            Login,
            Email,
            DisplayName,
            Avatars?.Select(a => a.ToDto()).ToList(),
            CurrentAvatar?.ToDto(),
            About
        );
    }
}