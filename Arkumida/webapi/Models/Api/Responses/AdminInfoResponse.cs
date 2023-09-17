using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with administrator info
/// </summary>
public class AdminInfoResponse
{
    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; }

    public AdminInfoResponse
    (
        string email
    )
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Admin email must not be empty.", nameof(email));
        }

        Email = email;
    }
}