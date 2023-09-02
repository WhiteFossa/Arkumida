using webapi.Models.Api.DTOs;

namespace webapi.Models;

/// <summary>
/// Creature's avatar
/// </summary>
public class Avatar
{
    /// <summary>
    /// ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Upload time (for ordering)
    /// </summary>
    public DateTime UploadTime { get; set; }

    /// <summary>
    /// File with avatar
    /// </summary>
    public File File { get; set; }

    /// <summary>
    /// Converts avatar to DTO
    /// </summary>
    public AvatarDto ToDto()
    {
        return new AvatarDto()
        {
            Id = Id,
            Name = Name,
            UploadTime = UploadTime,
            FileId = File.Id
        };
    }
}