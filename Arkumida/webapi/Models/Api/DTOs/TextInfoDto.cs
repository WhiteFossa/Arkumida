using System.Text.Json.Serialization;
using webapi.Models.Enums;

namespace webapi.Models.Api.DTOs;

public class TextInfoDto : IdedEntityDto
{
    /// <summary>
    /// Text authors
    /// </summary>
    [JsonPropertyName("authors")]
    public IList<CreatureDto> Authors { get; private set; }
    
    /// <summary>
    /// Text translators
    /// </summary>
    [JsonPropertyName("translators")]
    public IList<CreatureDto> Translators { get; private set; }
    
    /// <summary>
    /// Text publisher
    /// </summary>
    [JsonPropertyName("publisher")]
    public CreatureDto Publisher { get; private set; }

    /// <summary>
    /// Text title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; private set; }
    
    /// <summary>
    /// Text add time
    /// </summary>
    [JsonPropertyName("addTime")]
    public DateTime AddTime { get; private set; }

    /// <summary>
    /// Views count
    /// </summary>
    [JsonPropertyName("viewsCount")]
    public long ViewsCount { get; private set; }
    
    /// <summary>
    /// Comments count
    /// </summary>
    [JsonPropertyName("commentsCount")]
    public int CommentsCount { get; private set; }
    
    /// <summary>
    /// Votes for
    /// </summary>
    [JsonPropertyName("votesFor")]
    public long VotesFor { get; private set; }
    
    /// <summary>
    /// Votes agains
    /// </summary>
    [JsonPropertyName("votesAgainst")]
    public long VotesAgainst { get; private set; }

    /// <summary>
    /// Text tags (including category tags)
    /// </summary>
    [JsonPropertyName("tags")]
    public IReadOnlyCollection<TextTagDto> Tags { get; private set; }
    
    /// <summary>
    /// Additional left icons
    /// </summary>
    [JsonPropertyName("leftIcons")]
    public IReadOnlyCollection<TextIconDto> LeftIcons { get; private set; }
    
    /// <summary>
    /// Additional right icons
    /// </summary>
    [JsonPropertyName("rightIcons")]
    public IReadOnlyCollection<TextIconDto> RightIcons { get; private set; }

    /// <summary>
    /// Short text description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; private set; }

    /// <summary>
    /// Text size in bytes
    /// </summary>
    [JsonPropertyName("sizeInBytes")]
    public int SizeInBytes { get; private set; }

    /// <summary>
    /// Text size in pages (initially made for comics)
    /// </summary>
    [JsonPropertyName("sizeInPages")]
    public int SizeInPages { get; private set; }
    
    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    [JsonPropertyName("isIncomplete")]
    public bool IsIncomplete { get; set; }

    public TextInfoDto
    (
        Guid id,
        string furryReadableId,
        IList<CreatureDto> authors,
        IList<CreatureDto> translators,
        CreatureDto publisher,
        string title,
        DateTime addTime,
        long viewsCount,
        int commentsCount,
        long votesFor,
        long votesAgainst,
        IReadOnlyCollection<TextTagDto> tags,
        IReadOnlyCollection<TextIconDto> leftIcons,
        IReadOnlyCollection<TextIconDto> rightIcons,
        string description,
        int sizeInBytes,
        int sizeInPages,
        bool isIncomplete
    ) : base(id, furryReadableId)
    {
        Authors = authors ?? throw new ArgumentNullException(nameof(authors), "Authors must not be null");
        if (!Authors.Any())
        {
            throw new ArgumentException("At least one author is required!", nameof(authors));
        }
        
        Translators = translators ?? throw new ArgumentNullException(nameof(translators), "Translators must not be null");
        Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher), "Publisher must not be null");

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must be populated.", nameof(title));
        }
        Title = title;
        
        AddTime = addTime;

        if (viewsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(viewsCount), "Views count must be positive");
        }
        ViewsCount = viewsCount;
        
        if (commentsCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(commentsCount), "Comments count must be positive");
        }
        CommentsCount = commentsCount;
        
        if (votesFor < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(votesFor), "Votes for must be positive");
        }
        VotesFor = votesFor;
        
        if (votesAgainst < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(votesAgainst), "Votes against must be positive");
        }
        VotesAgainst = votesAgainst;

        Tags = tags ?? throw new ArgumentNullException(nameof(tags), "Tags must not be null");
        LeftIcons = leftIcons ?? throw new ArgumentNullException(nameof(leftIcons), "Left icons must not be null");
        RightIcons = rightIcons ?? throw new ArgumentNullException(nameof(rightIcons), "Right icons must not be null");
        Description = description; // Unfortunately there is some stories in old FT DB, where descriptions are empty
        
        if (sizeInBytes < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeInBytes), "Size in bytes must be positive."); // 0 is OK for comics
        }
        SizeInBytes = sizeInBytes;
        
        if (sizeInPages < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeInPages), "Size in pages must be positive."); // 0 is OK for non-comics
        }
        SizeInPages = sizeInPages;

        IsIncomplete = isIncomplete;
    }
}