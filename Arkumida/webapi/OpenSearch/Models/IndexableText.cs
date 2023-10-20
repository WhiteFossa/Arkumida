using OpenSearch.Client;

namespace webapi.OpenSearch.Models;

/// <summary>
/// Text, indexable to OpenSearch
/// </summary>
public class IndexableText : IIndexableEntity
{
    public static string IndexName => "texts";
    
    /// <summary>
    /// Creature ID (the same as in DB, but serialized by OpenSearchGuidHelper)
    /// </summary>
    public string DbId { get; set; }

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
    public List<string> AuthorsDbIds { get; set; }

    /// <summary>
    /// Translators DB IDs (may be empty)
    /// </summary>
    public List<string> TranslatorsDbIds { get; set; }

    /// <summary>
    /// Publisher DB ID
    /// </summary>
    public string PublisherDbId { get; set; }

    /// <summary>
    /// Tags DB IDs
    /// </summary>
    public List<string> TagsDbIds { get; set; }
}