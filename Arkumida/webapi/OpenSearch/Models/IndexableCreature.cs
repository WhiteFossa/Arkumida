namespace webapi.OpenSearch.Models;

/// <summary>
/// OpenSearch-indexable creature
/// </summary>
public class IndexableCreature : IIndexableEntity
{
    /// <summary>
    /// Creature ID (the same as in DB)
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string DisplayName { get; set; }

    public string GetIndexName()
    {
        return "creatures";
    }
}