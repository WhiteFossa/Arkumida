using webapi.Models.Api.DTOs;

namespace webapi.Models;

/// <summary>
/// User (mapped from UserDbo)
/// </summary>
public class Creature
{
    /// <summary>
    /// User ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// User's visible name
    /// </summary>
    public string DisplayName { get; set; }

    public Creature
    (
        Guid id,
        string login,
        string email,
        string displayName
    )
    {
        // All fields except Id might be null when data is coming from the frontend
        Id = id;

        Login = login;
        Email = email;
        DisplayName = displayName;
    }
    
    /// <summary>
    /// Convert to CreatureDto
    /// </summary>
    public CreatureDto ToDto()
    {
        return new CreatureDto(Id, "not_ready", DisplayName, Login, Email);
    }
}