using OpenSearch.Client;

namespace webapi.OpenSearch.Models;

/// <summary>
/// Text, indexable to OpenSearch
/// </summary>
public class IndexableText : IIndexableEntity
{
    public static string IndexName => "texts";
    
    /// <summary>
    /// Creature ID
    /// </summary>
    public Guid DbId { get; set; }

    /// <summary>
    /// When text was updated last time
    /// </summary>
    public DateTime LastUpdateTime { get; set; }
    
    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Raw text content (without title, tags and so on - to avoid accidental search on it)
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Authors DB IDs
    /// </summary>
    public List<Guid> AuthorsDbIds { get; set; }

    /// <summary>
    /// Translators DB IDs (may be empty)
    /// </summary>
    public List<Guid> TranslatorsDbIds { get; set; }

    /// <summary>
    /// Publisher DB ID
    /// </summary>
    public Guid PublisherDbId { get; set; }

    /// <summary>
    /// Tags DB IDs
    /// </summary>
    public List<Guid> TagsDbIds { get; set; }
}