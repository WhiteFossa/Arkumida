using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Enums;

namespace furtails_importer.WebClientStuff.Dtos;

public class RegistrationResultDto
{
    /// <summary>
    /// Registered user ID
    /// </summary>
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Registration result
    /// </summary>
    [JsonPropertyName("result")]
    public UserRegistrationResult Result { get; set; }
}