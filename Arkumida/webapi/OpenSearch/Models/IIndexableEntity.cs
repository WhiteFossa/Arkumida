namespace webapi.OpenSearch.Models;

/// <summary>
/// Basic class for everything, which can be indexed by OpenSearch
/// </summary>
public interface IIndexableEntity
{
    /// <summary>
    /// Get entity's index name
    /// </summary>
    public string GetIndexName();
}