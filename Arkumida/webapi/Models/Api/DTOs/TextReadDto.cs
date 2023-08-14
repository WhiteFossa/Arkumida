using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// DTO with information, required to display read page
/// </summary>
public class TextReadDto : IdedEntityDto
{
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
    /// Text tags
    /// </summary>
    [JsonPropertyName("tags")]
    public IList<TagDto> Tags { get; set; }
    
    /// <summary>
    /// Text authors
    /// </summary>
    [JsonPropertyName("authors")]
    public IList<CreatureDto> Authors { get; private set; }
    
    /// <summary>
    /// Text translator
    /// </summary>
    [JsonPropertyName("translators")]
    public IList<CreatureDto> Translators { get; private set; }
    
    /// <summary>
    /// Text publisher
    /// </summary>
    [JsonPropertyName("publisher")]
    public CreatureDto Publisher { get; private set; }

    /// <summary>
    /// Text illustrations
    /// </summary>
    [JsonPropertyName("illustrations")]
    public IReadOnlyCollection<TextFileDto> Illustrations { get; private set; }

    /// <summary>
    /// Total pages count (note, that first page have index of 1, not of 0)
    /// </summary>
    public int PagesCount { get; private set; }

    #region Rendered files

    /// <summary>
    /// Rendered plain text file. It always exist, but may be useless (for comics)
    /// </summary>
    [JsonPropertyName("plainTextFile")]
    public FileInfoDto PlainTextFile { get; private set; }

    #endregion

    public TextReadDto
    (
        Guid id,
        string furryReadableId,
        DateTime createTime,
        DateTime lastUpdateTime,
        string title,
        string description,
        IReadOnlyCollection<TagDto> tags,
        IList<CreatureDto> authors,
        IList<CreatureDto> translators,
        CreatureDto publisher,
        IReadOnlyCollection<TextFileDto> illustrations,
        int pagesCount,
        FileInfoDto plainTextFile
    ) : base (id, furryReadableId)
    {
        CreateTime = createTime;
        LastUpdateTime = lastUpdateTime;

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must be populated.", nameof(title));
        }
        Title = title;
        
        Description = description; // May be empty
        Tags = (tags ?? throw new ArgumentNullException(nameof(tags), "Tags mustn't be null.")).ToList();
        
        Authors = authors ?? throw new ArgumentNullException(nameof(authors), "Authors must not be null.");
        if (!Authors.Any())
        {
            throw new ArgumentException("At least one author must be specified.", nameof(authors));
        }
        
        Translators = translators ?? throw new ArgumentNullException(nameof(translators), "Translators must not be null.");
        Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher), "Publisher must not be null.");
        
        Illustrations = illustrations ?? throw new ArgumentNullException(nameof(illustrations), "Illustrations must not be null.");

        if (pagesCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pagesCount), pagesCount, "Text must have at least one page.");
        }
        PagesCount = pagesCount;

        PlainTextFile = plainTextFile ?? throw new ArgumentNullException(nameof(plainTextFile), "Plain text rendered file must not be null!");
    }
}