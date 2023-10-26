namespace webapi.OpenSearch.Models;

/// <summary>
/// Basic class for everything, which can be indexed by OpenSearch
/// </summary>
public interface IIndexableEntity
{
    /// <summary>
    /// Entity index name
    /// </summary>
    static abstract string IndexName { get; }
}