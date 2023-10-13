namespace webapi.OpenSearch.Models;

/// <summary>
/// Tag, indexable to OpenSearch
/// </summary>
public class IndexableTag : IIndexableEntity
{
    public static string IndexName => "tags";
    
    /// <summary>
    /// Tag ID (same as in DB)
    /// </summary>
    public Guid DbId { get; set; }

    /// <summary>
    /// Tag name
    /// </summary>
    public string Name { get; set; }
}