using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// DTO with information, required to display read page
/// </summary>
public class TextReadDto : IdedEntityDto
{
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
    /// Text tags
    /// </summary>
    [JsonPropertyName("tags")]
    public IList<TagDto> Tags { get; set; }
    
    /// <summary>
    /// Text author
    /// </summary>
    [JsonPropertyName("author")]
    public CreatureDto Author { get; private set; }
    
    /// <summary>
    /// Text translator (may be null)
    /// </summary>
    [JsonPropertyName("translator")]
    public CreatureDto Translator { get; private set; }
    
    /// <summary>
    /// Text publisher
    /// </summary>
    [JsonPropertyName("publisher")]
    public CreatureDto Publisher { get; private set; }

    /// <summary>
    /// Text illustrations
    /// </summary>
    [JsonPropertyName("illustrations")]
    public IReadOnlyCollection<TextFileDto> Illustrations { get; private set; }

    public TextReadDto
    (
        Guid id,
        string furryReadableId,
        DateTime createTime,
        DateTime lastUpdateTime,
        string title,
        string description,
        IReadOnlyCollection<TagDto> tags,
        CreatureDto author,
        CreatureDto translator,
        CreatureDto publisher,
        IReadOnlyCollection<TextFileDto> illustrations
    ) : base (id, furryReadableId)
    {
        CreateTime = createTime;
        LastUpdateTime = lastUpdateTime;

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must be populated.", nameof(title));
        }
        Title = title;
        
        Description = description; // May be empty
        Tags = (tags ?? throw new ArgumentNullException(nameof(tags), "Tags mustn't be null.")).ToList();
        Author = author ?? throw new ArgumentNullException(nameof(author), "Author must not be null");
        Translator = translator;
        Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher), "Publisher must not be null");
        Illustrations = illustrations ?? throw new ArgumentNullException(nameof(illustrations), "Illustrations must not be null");
    }
}