using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// File upload response
/// </summary>
public class UploadFileResponse
{
    [JsonPropertyName("fileInfo")]
    public FileInfoDto FileInfo { get; private set; }

    public UploadFileResponse
    (
        FileInfoDto fileInfo
    )
    {
        FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo), "File info must not be null!");
    }
}