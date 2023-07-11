using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

/// <summary>
/// DTO with information on file
/// </summary>
public class FileInfoDto
{
    /// <summary>
    /// File ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Filename
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}