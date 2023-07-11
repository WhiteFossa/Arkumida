using System.Text.Json.Serialization;
using furtails_importer.WebClientStuff.Dtos;

namespace furtails_importer.WebClientStuff.Responses;

/// <summary>
/// File upload response
/// </summary>
public class UploadFileResponse
{
    [JsonPropertyName("fileInfo")]
    public FileInfoDto FileInfo { get; set; }
}