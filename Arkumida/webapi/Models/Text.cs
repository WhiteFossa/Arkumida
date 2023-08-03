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
    /// Text author
    /// </summary>
    public Creature Author { get; set; }

    /// <summary>
    /// Text translator
    /// </summary>
    public Creature Translator { get; set; }

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
            Author.ToDto(),
            Translator?.ToDto(),
            Publisher.ToDto()
        );
    }
}