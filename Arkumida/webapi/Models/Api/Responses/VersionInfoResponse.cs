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

    public VersionInfoResponse
    (
        string versionString
    )
    {
        if (string.IsNullOrWhiteSpace(versionString))
        {
            throw new ArgumentException("Version string must not be empty.", nameof(versionString));
        }

        VersionString = versionString;
    }
}