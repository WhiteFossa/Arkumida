using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Avatar
/// </summary>
public class AvatarDto
{
    /// <summary>
    /// ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Upload time (for ordering)
    /// </summary>
    [JsonPropertyName("uploadTime")]
    public DateTime UploadTime { get; set; }

    /// <summary>
    /// ID of avatar file
    /// </summary>
    [JsonPropertyName("fileId")]
    public Guid FileId { get; set; }

    /// <summary>
    /// Converts to avatar model (of course file is not loaded, only ID is provided)
    /// </summary>
    public Avatar ToModel()
    {
        return new Avatar()
        {
            Id = Id,
            Name = Name,
            UploadTime = UploadTime,
            File = new File() { Id = FileId}
        };
    }
}