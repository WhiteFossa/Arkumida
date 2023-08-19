using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// User
/// </summary>
public class CreatureDto : IdedEntityDto
{
    /// <summary>
    /// Creature name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; private set; }

    /// <summary>
    /// Creature login
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; private set; }

    /// <summary>
    /// Creature email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; private set; }

    public CreatureDto
    (
        Guid id,
        string furryReadableId,
        string name,
        string login,
        string email
    ) : base(id, furryReadableId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be populated!", nameof(name));
        }
        Name = name;

        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Login must be populated!", nameof(login));
        }
        Login = login;
        
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email must be populated!", nameof(email));
        }
        Email = email;
    }

    /// <summary>
    /// Build an user based on creature DTO. Please note that not all fields can be filled
    /// </summary>
    public Creature ToUser()
    {
        return new Creature(Id, Login, string.Empty, Email, Name);
    }
}