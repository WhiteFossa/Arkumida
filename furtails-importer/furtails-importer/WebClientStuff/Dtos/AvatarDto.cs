using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

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
}