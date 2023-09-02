using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Avatar creation response
/// </summary>
public class CreateAvatarResponse
{
    /// <summary>
    /// Created avatar
    /// </summary>
    [JsonPropertyName("avatar")]
    public AvatarDto Avatar { get; private set; }

    public CreateAvatarResponse
    (
        AvatarDto avatar
    )
    {
        Avatar = avatar ?? throw new ArgumentNullException(nameof(avatar), "Avatar must be populated!");
    }
}