using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Author
/// </summary>
public class AuthorDto : IdedEntityDto
{
    /// <summary>
    /// Author name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; private set; }

    public AuthorDto
    (
        Guid id,
        string humanReadableId,
        string name
    ) : base(id, humanReadableId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be populated", nameof(name));
        }

        Name = name;
    }
}