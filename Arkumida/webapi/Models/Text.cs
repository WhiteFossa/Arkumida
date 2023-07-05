using webapi.Models.Api.DTOs;
using webapi.Services.Abstract;

namespace webapi.Models;

/// <summary>
/// One text
/// </summary>
public class Text
{
    /// <summary>
    /// Text ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// When text was created
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// When text was updated last time
    /// </summary>
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// Text title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Text description (for text info)
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Text sections
    /// </summary>
    public IList<TextSection> Sections { get; set; }
    
    /// <summary>
    /// How many times text was read
    /// </summary>
    public long ReadsCount { get; set; }
    
    /// <summary>
    /// Votes count for text
    /// </summary>
    public long VotesCount { get; set; }

    /// <summary>
    /// Votes pro
    /// </summary>
    public long VotesPlus { get; set; }

    /// <summary>
    /// Votes contra
    /// </summary>
    public long VotesMinus { get; set; }
    
    /// <summary>
    /// Tags
    /// </summary>
    public IList<Tag> Tags { get; set; }
    
    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    public bool IsIncomplete { get; set; }

    public TextDto ToDto(ITextsService textsService)
    {
        return new TextDto
        (
            Id,
            CreateTime,
            LastUpdateTime,
            Title,
            Description,
            Sections.Select(s => s.ToDto(textsService)).ToList(),
            ReadsCount,
            VotesCount,
            VotesPlus,
            VotesMinus,
            Tags.Select(t => t.ToTagDto()).ToList(),
            IsIncomplete
        );
    }
}