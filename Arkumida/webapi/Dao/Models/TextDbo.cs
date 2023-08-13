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
    /// Text pages
    /// </summary>
    public IList<TextPageDbo> Pages { get; set; }

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
    /// Text's tags
    /// </summary>
    public IList<TagDbo> Tags { get; set; }

    /// <summary>
    /// If true, then text is not complete yet
    /// </summary>
    public bool IsIncomplete { get; set; }

    /// <summary>
    /// Files, attached to text
    /// </summary>
    public IList<TextFileDbo> TextFiles { get; set; }

    /// <summary>
    /// Text authors
    /// </summary>
    public IList<CreatureDbo> Authors { get; set; }

    /// <summary>
    /// Text translator
    /// </summary>
    public IList<CreatureDbo> Translators { get; set; }

    /// <summary>
    /// Text publisher
    /// </summary>
    public CreatureDbo Publisher { get; set; }

    /// <summary>
    /// Rendered texts (plaintext, PDF etc)
    /// </summary>
    public IList<RenderedTextDbo> RenderedTexts { get; set; }
}