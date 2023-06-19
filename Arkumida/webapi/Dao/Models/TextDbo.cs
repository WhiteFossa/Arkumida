using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

public class TextDbo
{
    /// <summary>
    /// Text ID
    /// </summary>
    [Key]
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
    public IList<TextSectionDbo> Sections { get; set; }

    /// <summary>
    /// How many times text was read
    /// </summary>
    public long ReadsCount { get; set; }

}