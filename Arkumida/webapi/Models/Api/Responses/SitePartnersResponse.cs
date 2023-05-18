using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response with site partners
/// </summary>
public class SitePartnersResponse
{
    /// <summary>
    /// Partners list
    /// </summary>
    [JsonPropertyName("partners")]
    public IReadOnlyCollection<SitePartnerDto> Partners { get; }

    public SitePartnersResponse
    (
        IReadOnlyCollection<SitePartnerDto> partners
    )
    {
        Partners = partners ?? throw new ArgumentNullException(nameof(partners), "Partners must be defined");
    }
}