namespace webapi.OpenSearch.Models;

/// <summary>
/// OpenSearch-indexable creature
/// </summary>
public class IndexableCreature : IIndexableEntity
{
    public static string IndexName => "creatures";
    
    /// <summary>
    /// Creature ID (the same as in DB)
    /// </summary>
    public Guid DbId { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string DisplayName { get; set; }
}