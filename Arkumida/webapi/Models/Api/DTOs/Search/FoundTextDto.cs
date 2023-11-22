using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs.Search;

/// <summary>
/// DTO for one found text. Very similar to TextDto, but definitely don't have a content
/// </summary>
public class FoundTextDto
{
    /// <summary>
    /// Text ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; private set; }

    /// <summary>
    /// When text was created
    /// </summary>
    [JsonPropertyName("createTime")]
    public DateTime CreateTime { get; private set; }

    /// <summary>
    /// When text was updated last time
    /// </summary>
    [JsonPropertyName("lastUpdateTime")]
    public DateTime LastUpdateTime { get; private set; }

    /// <summary>
    /// Text title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; private set; }

    /// <summary>
    /// Text description (for text info)
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; private set; }
    
    /// <summary>
    /// How many times text was read
    /// </summary>
    [JsonPropertyName("readsCount")]
    public long ReadsCount { get; private set; }

    /// <summary>
    /// Text tags
    /// </summary>
    [JsonPropertyName("tags")]
    public IList<TagDto> Tags { get; private set; }
    
    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    [JsonPropertyName("isIncomplete")]
    public bool IsIncomplete { get; private set; }

    /// <summary>
    /// Text authors
    /// </summary>
    [JsonPropertyName("authors")]
    public IList<CreatureWithProfileDto> Authors { get; private set; }
    
    /// <summary>
    /// Text translators
    /// </summary>
    [JsonPropertyName("translators")]
    public IList<CreatureWithProfileDto> Translators { get; private set; }
    
    /// <summary>
    /// Text publisher
    /// </summary>
    [JsonPropertyName("publisher")]
    public CreatureWithProfileDto Publisher { get; private set; }
    
    public FoundTextDto
    (
        Guid id,
        DateTime createTime,
        DateTime lastUpdateTime,
        string title,
        string description,
        long readsCount,
        IReadOnlyCollection<TagDto> tags,
        bool isIncomplete,
        IList<CreatureWithProfileDto> authors,
        IList<CreatureWithProfileDto> translators,
        CreatureWithProfileDto publisher
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
        
        Description = description; // May be empty

        if (readsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(readsCount), "Reads count must be non-negative.");
        }
        ReadsCount = readsCount;
        
        Tags = (tags ?? throw new ArgumentNullException(nameof(tags), "Tags mustn't be null.")).ToList();

        IsIncomplete = isIncomplete;

        Authors = authors ?? throw new ArgumentNullException(nameof(authors), "Authors must be specified.");
        if (!Authors.Any())
        {
            throw new ArgumentException("At least one author is required!", nameof(authors));
        }
        
        // We may have empty translators list, but still not null
        Translators = translators ?? throw new ArgumentNullException(nameof(translators), "Translators must be specified.");
        
        Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher), "Publisher must be specified.");
    }
}