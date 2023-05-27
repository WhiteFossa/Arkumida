using System.Text.Json.Serialization;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Information about site version
/// </summary>
public class VersionInfoResponse
{
    /// <summary>
    /// Version string
    /// </summary>
    [JsonPropertyName("versionString")]
    public string VersionString { get; }

    /// <summary>
    /// Site sources URL
    /// </summary>
    [JsonPropertyName("sourcesUrl")]
    public string SourcesUrl { get; }

    public VersionInfoResponse
    (
        string versionString,
        string sourcesUrl
    )
    {
        if (string.IsNullOrWhiteSpace(versionString))
        {
            throw new ArgumentException("Version string must not be empty.", nameof(versionString));
        }
        
        if (string.IsNullOrWhiteSpace(sourcesUrl))
        {
            throw new ArgumentException("Sources URL must not be empty.", nameof(sourcesUrl));
        }

        VersionString = versionString;
        SourcesUrl = sourcesUrl;
    }
}