using System.Text.Json.Serialization;
using webapi.Models.Api.Enums;

namespace webapi.Models.Api.DTOs;

public class TextInfoDto : IdedEntityDto
{
    /// <summary>
    /// Text author
    /// </summary>
    [JsonPropertyName("author")]
    public AuthorDto Author { get; private set; }

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
    public int ViewsCount { get; private set; }
    
    /// <summary>
    /// Comments count
    /// </summary>
    [JsonPropertyName("commentsCount")]
    public int CommentsCount { get; private set; }
    
    /// <summary>
    /// Votes for
    /// </summary>
    [JsonPropertyName("votesFor")]
    public int VotesFor { get; private set; }
    
    /// <summary>
    /// Votes agains
    /// </summary>
    [JsonPropertyName("votesAgainst")]
    public int VotesAgainst { get; private set; }

    /// <summary>
    /// Text tags
    /// </summary>
    [JsonPropertyName("tags")]
    public IReadOnlyCollection<TextTagDto> Tags { get; private set; }

    /// <summary>
    /// Text type
    /// </summary>
    [JsonPropertyName("type")]
    public TextType Type { get; private set; }
    
    /// <summary>
    /// Special text type
    /// </summary>
    [JsonPropertyName("specialType")]
    public SpecialTextType SpecialType { get; private set; }

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

    public TextInfoDto
    (
        Guid id,
        string humanReadableId,
        AuthorDto author,
        string title,
        DateTime addTime,
        int viewsCount,
        int commentsCount,
        int votesFor,
        int votesAgainst,
        IReadOnlyCollection<TextTagDto> tags,
        TextType type,
        SpecialTextType specialType,
        IReadOnlyCollection<TextIconDto> leftIcons,
        IReadOnlyCollection<TextIconDto> rightIcons
    ) : base(id, humanReadableId)
    {
        Author = author ?? throw new ArgumentNullException(nameof(author), "Author must not be null");

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
        Type = type;
        SpecialType = specialType;

        LeftIcons = leftIcons ?? throw new ArgumentNullException(nameof(leftIcons), "Left icons must not be null");
        RightIcons = rightIcons ?? throw new ArgumentNullException(nameof(rightIcons), "Right icons must not be null");
    }
}