using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Information about how to help Redgerra
/// </summary>
public class DonateToRegderraInfoResponse
{
    /// <summary>
    /// Donate page URL
    /// </summary>
    [JsonPropertyName("donatePageUrl")]
    public string DonatePageUrl { get; }
    
    /// <summary>
    /// Donate card number
    /// </summary>
    [JsonPropertyName("donateCardNumber")]
    public string DonateCardNumber { get; }

    public DonateToRegderraInfoResponse
    (
        string donatePageUrl,
        string donateCardNumber
    )
    {
        if (string.IsNullOrWhiteSpace(donatePageUrl))
        {
            throw new ArgumentException("Donate page URL must be populated.", nameof(donatePageUrl));
        }
        DonatePageUrl = donatePageUrl;

        if (string.IsNullOrWhiteSpace(donateCardNumber))
        {
            throw new ArgumentException("Donate card number must be populated.", nameof(donateCardNumber));
        }
        DonateCardNumber = donateCardNumber;
    }
}