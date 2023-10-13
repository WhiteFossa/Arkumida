namespace webapi.OpenSearch.Models;

/// <summary>
/// OpenSearch-indexable creature
/// </summary>
public class IndexableCreature : IIndexableEntity
{
    /// <summary>
    /// Creature ID (the same as in DB)
    /// </summary>
    public Guid DbId { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string DisplayName { get; set; }

    public static string IndexName => "creatures";
}