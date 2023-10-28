using OpenSearch.Client;

namespace webapi.OpenSearch.Models;

/// <summary>
/// Tag, indexable to OpenSearch
/// </summary>
public class IndexableTag : IIndexableEntity
{
    public static string IndexName => "tags";
    
    /// <summary>
    /// Creature ID
    /// </summary>
    public Guid DbId { get; set; }

    /// <summary>
    /// Tag name
    /// </summary>
    public string Name { get; set; }
}