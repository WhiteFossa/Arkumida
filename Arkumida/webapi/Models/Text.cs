using webapi.Models.Api.DTOs;
using webapi.Services.Abstract;

namespace webapi.Models;

/// <summary>
/// One text
/// TODO: Add constructor, important
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
    /// Text pages
    /// </summary>
    public IList<TextPage> Pages { get; set; }
    
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
    
    /// <summary>
    /// Files, attached to text
    /// </summary>
    public IList<TextFile> TextFiles { get; set; }
    
    /// <summary>
    /// Text authors
    /// </summary>
    public IList<Creature> Authors { get; set; }

    /// <summary>
    /// Text translators
    /// </summary>
    public IList<Creature> Translators { get; set; }

    /// <summary>
    /// Text publisher
    /// </summary>
    public Creature Publisher { get; set; }

    public TextDto ToDto(ITextsService textsService)
    {
        return new TextDto
        (
            Id,
            CreateTime,
            LastUpdateTime,
            Title,
            Description,
            Pages.Select(p => p.ToDto(this.TextFiles.ToList(), textsService)).ToList(),
            ReadsCount,
            VotesCount,
            VotesPlus,
            VotesMinus,
            Tags.Select(t => t.ToTagDto()).ToList(),
            IsIncomplete,
            Authors.Select(a => a.ToDto()).ToList(),
            Translators.Select(t => t.ToDto()).ToList(),
            Publisher.ToDto()
        );
    }
}