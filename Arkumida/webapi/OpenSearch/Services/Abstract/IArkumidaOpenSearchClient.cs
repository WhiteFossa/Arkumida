using webapi.OpenSearch.Models;

namespace webapi.OpenSearch.Services.Abstract;

/// <summary>
/// Client to work with OpenSearch
/// </summary>
public interface IArkumidaOpenSearchClient
{
    /// <summary>
    /// Connect to OpenSearch using settings from appsettings.json If we are already connected - does nothing
    /// </summary>
    void Connect();

    #region Creatures
    
    /// <summary>
    /// Add creature to OpenSearch. Returns OpenSearch (not creature!) ID
    /// </summary>
    Task<string> IndexCreatureAsync(IndexableCreature creatureToIndex);

    /// <summary>
    /// Get OpenSearch creature ID by creature DB ID
    /// </summary>
    Task<string> GetCreatureOpenSearchIdAsync(Guid creatureDbId);

    /// <summary>
    /// Get indexable creature by DB ID
    /// </summary>
    Task<IndexableCreature> GetCreatureByDbIdAsync(Guid creatureDbId);

    /// <summary>
    /// Update existing creature
    /// </summary>
    Task UpdateCreatureAsync(IndexableCreature creature);

    #endregion
}