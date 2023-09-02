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
    
    public CreatureWithProfile
    (
        Guid id,
        string login,
        string email,
        string displayName,
        IList<Avatar> avatars,
        Avatar currentAvatar
    ) : base(id, login, email)
    {
        DisplayName = displayName;
        Avatars = avatars;
        CurrentAvatar = currentAvatar;
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
            CurrentAvatar?.ToDto()
        );
    }
}