using webapi.Models.Api.DTOs;

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

    public TextDto ToDto()
    {
        return new TextDto(Id, CreateTime, LastUpdateTime, Title, Description, Sections.Select(s => s.ToDto()).ToList());
    }
}