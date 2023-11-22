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
    public IList<CreatureWithProfile> Authors { get; set; }

    /// <summary>
    /// Text translators
    /// </summary>
    public IList<CreatureWithProfile> Translators { get; set; }

    /// <summary>
    /// Text publisher
    /// </summary>
    public CreatureWithProfile Publisher { get; set; }

    public TextDto ToDto(ITextUtilsService textUtilsService)
    {
        return new TextDto
        (
            Id,
            CreateTime,
            LastUpdateTime,
            Title,
            Description,
            Pages.Select(p => p.ToDto(this.TextFiles.ToList(), textUtilsService)).ToList(),
            Tags.Select(t => t.ToTagDto()).ToList(),
            IsIncomplete,
            Authors.Select(a => a.ToDto()).ToList(),
            Translators.Select(t => t.ToDto()).ToList(),
            Publisher.ToDto()
        );
    }
}