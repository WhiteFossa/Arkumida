using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Requests;

public class CreateAvatarRequest
{
    /// <summary>
    /// Avatar to create
    /// </summary>
    [JsonPropertyName("avatar")]
    public AvatarDto Avatar { get; set; }
}