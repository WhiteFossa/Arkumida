using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// User
/// </summary>
public class CreatureDto : IdedEntityDto
{
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
        string login,
        string email
    ) : base(id, furryReadableId)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Login must be populated!", nameof(login));
        }
        Login = login;

        Email = email ?? throw new ArgumentNullException(nameof(email), "Email must not be null, at least empty string required!");
    }

    /// <summary>
    /// Build an user based on creature DTO. Please note that not all fields can be filled
    /// </summary>
    public Creature ToUser()
    {
        return new Creature(Id, Login, Email);
    }
}