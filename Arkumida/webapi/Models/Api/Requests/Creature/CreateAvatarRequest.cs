using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Requests.Creature;

/// <summary>
/// Request to create user's avatar
/// </summary>
public class CreateAvatarRequest
{
    /// <summary>
    /// Avatar to create
    /// </summary>
    [JsonPropertyName("avatar")]
    public AvatarDto Avatar { get; set; }
}