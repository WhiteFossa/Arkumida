using webapi.Models.Api.DTOs;

namespace webapi.Models;

/// <summary>
/// Creature
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
    
    public Creature
    (
        Guid id,
        string login,
        string email
    )
    {
        // All fields except Id might be null when data is coming from the frontend
        Id = id;

        Login = login;
        Email = email;
    }
    
    /// <summary>
    /// Convert to CreatureDto
    /// </summary>
    public CreatureDto ToDto()
    {
        return new CreatureDto(Id, "not_ready", Login, Email);
    }
}