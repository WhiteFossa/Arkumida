using OpenSearch.Client;

namespace webapi.OpenSearch.Models;

/// <summary>
/// Tag, indexable to OpenSearch
/// </summary>
public class IndexableTag : IIndexableEntity
{
    public static string IndexName => "tags";
    
    /// <summary>
    /// Creature ID (the same as in DB, but serialized by OpenSearchGuidHelper)
    /// </summary>
    public string DbId { get; set; }

    /// <summary>
    /// Tag name
    /// </summary>
    public string Name { get; set; }
}