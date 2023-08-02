using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class LoginResultDto
{
    /// <summary>
    /// Is credentials correct?
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Token
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; }

    /// <summary>
    /// Token expiration date and time
    /// </summary>
    [JsonPropertyName("expiration")]
    public DateTime ExpirationTime { get; set; }
}