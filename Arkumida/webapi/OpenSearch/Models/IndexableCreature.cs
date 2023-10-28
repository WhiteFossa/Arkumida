using OpenSearch.Client;

namespace webapi.OpenSearch.Models;

/// <summary>
/// OpenSearch-indexable creature
/// </summary>
public class IndexableCreature : IIndexableEntity
{
    public static string IndexName => "creatures";
    
    /// <summary>
    /// Creature ID
    /// </summary>
    public Guid DbId { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string DisplayName { get; set; }
}