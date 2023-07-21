using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// DTO with text's file
/// </summary>
public class TextFileDto
{
    /// <summary>
    /// Text file ID (do NOT use it for downloading, use File.Id instead)
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; private set; }

    /// <summary>
    /// Filename
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; private set; }

    /// <summary>
    /// Actual file
    /// </summary>
    [JsonPropertyName("file")]
    public FileInfoDto File { get; private set; }

    public TextFileDto
    (
        Guid id,
        string name,
        FileInfoDto file
    )
    {
        Id = id;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("File name must be populated!", nameof(name));
        }
        Name = name;

        File = file ?? throw new ArgumentNullException(nameof(file), "File must not be null!");
    }
}