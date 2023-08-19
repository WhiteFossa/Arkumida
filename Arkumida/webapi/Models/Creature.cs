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
    /// Plaintext password, it is used to send notifications to creatures about "You are registered on Arkumida now, your password is...".
    /// After first successful login it should be wiped-out
    /// </summary>
    public string OneTimePlaintextPassword { get; set; }
    
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
        string oneTimePlaintextPassword,
        string email,
        string displayName
    )
    {
        // All fields except Id might be null when data is coming from the frontend
        Id = id;

        Login = login;
        OneTimePlaintextPassword = oneTimePlaintextPassword;
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