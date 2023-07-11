using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Requests;

/// <summary>
/// Request to add already uploaded files to texts
/// </summary>
public class AddFileToTextRequest
{
    /// <summary>
    /// Add file to this text
    /// </summary>
    [JsonPropertyName("textId")]
    public Guid TextId { get; set; }

    /// <summary>
    /// Add this file to text
    /// </summary>
    [JsonPropertyName("fileId")]
    public Guid FileId { get; set; }

    /// <summary>
    /// Add file under this name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}