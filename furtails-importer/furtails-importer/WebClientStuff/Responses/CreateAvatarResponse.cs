using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

public class CreateAvatarResponse
{
    /// <summary>
    /// Created avatar
    /// </summary>
    [JsonPropertyName("avatar")]
    public AvatarDto Avatar { get;  set; }
}