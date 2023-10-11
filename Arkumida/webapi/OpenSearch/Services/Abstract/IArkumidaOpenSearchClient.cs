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

    /// <summary>
    /// Add creature to OpenSearch. Returns OpenSearch (not creature!) ID
    /// </summary>
    Task<string> IndexCreatureAsync(IndexableCreature creatureToIndex);
}