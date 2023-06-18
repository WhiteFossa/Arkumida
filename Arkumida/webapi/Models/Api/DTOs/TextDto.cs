using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Text DTO
/// </summary>
public class TextDto
{
    /// <summary>
    /// Text ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// When text was created
    /// </summary>
    [JsonPropertyName("createTime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// When text was updated last time
    /// </summary>
    [JsonPropertyName("lastUpdateTime")]
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// Text title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// Text description (for text info)
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Text sections
    /// </summary>
    [JsonPropertyName("sections")]
    public IList<TextSectionDto> Sections { get; set; }

    public TextDto()
    {
        
    }

    public TextDto
    (
        Guid id,
        DateTime createTime,
        DateTime lastUpdateTime,
        string title,
        string description,
        IReadOnlyCollection<TextSectionDto> sections
    )
    {
        Id = id;
        CreateTime = createTime;
        LastUpdateTime = lastUpdateTime;

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must be populated.", nameof(title));
        }
        Title = title;

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description must be populated.", nameof(description));
        }
        Description = description;

        Sections = (sections ?? throw new ArgumentNullException(nameof(sections), "Sections mustn't be null.")).ToList();
    }

    public Text ToText()
    {
        return new Text()
        {
            Id = Id,
            CreateTime = CreateTime,
            LastUpdateTime = LastUpdateTime,
            Title = Title,
            Description = Description,
            Sections = Sections.Select(s => s.ToTextSection()).ToList()
        };
    }
}