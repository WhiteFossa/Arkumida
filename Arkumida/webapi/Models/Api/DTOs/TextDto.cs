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
    
    /// <summary>
    /// How many times text was read
    /// </summary>
    [JsonPropertyName("readsCount")]
    public long ReadsCount { get; set; }
    
    /// <summary>
    /// Votes count for text
    /// </summary>
    [JsonPropertyName("votesCount")]
    public long VotesCount { get; set; }

    /// <summary>
    /// Votes pro
    /// </summary>
    [JsonPropertyName("votesPlus")]
    public long VotesPlus { get; set; }

    /// <summary>
    /// Votes contra
    /// </summary>
    [JsonPropertyName("votesMinus")]
    public long VotesMinus { get; set; }

    /// <summary>
    /// Text tags
    /// </summary>
    [JsonPropertyName("tags")]
    public IList<TagDto> Tags { get; set; }
    
    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    [JsonPropertyName("isIncomplete")]
    public bool IsIncomplete { get; set; }

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
        IReadOnlyCollection<TextSectionDto> sections,
        long readsCount,
        long votesCount,
        long votesPlus,
        long votesMinus,
        IReadOnlyCollection<TagDto> tags,
        bool isIncomplete
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
        Sections = (sections ?? throw new ArgumentNullException(nameof(sections), "Sections mustn't be null.")).ToList();

        if (readsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(readsCount), "Reads count must be non-negative.");
        }
        ReadsCount = readsCount;
        
        if (votesCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(votesCount), "Votes count must be non-negative.");
        }
        VotesCount = votesCount;
        
        if (votesPlus < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(votesPlus), "Votes plus must be non-negative.");
        }
        VotesPlus = votesPlus;
        
        if (votesMinus < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(votesMinus), "Votes minus must be non-negative.");
        }
        VotesMinus = votesMinus;
        
        Tags = (tags ?? throw new ArgumentNullException(nameof(tags), "Tags mustn't be null.")).ToList();

        IsIncomplete = isIncomplete;
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
            Sections = Sections.Select(s => s.ToTextSection()).ToList(),
            ReadsCount = ReadsCount,
            VotesCount = VotesCount,
            VotesPlus = VotesPlus,
            VotesMinus = VotesMinus,
            Tags = Tags.Select(t => t.ToTag()).ToList(),
            IsIncomplete = IsIncomplete
        };
    }
}