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
    /// Add creature to OpenSearch. Returns OpenSearch (not DB) ID
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
    
    #region Tags

    /// <summary>
    /// Add tag to OpenSearch. Returns OpenSearch (not DB) ID
    /// </summary>
    Task<string> IndexTagAsync(IndexableTag tagToIndex);

    #endregion
    
    #region Texts

    /// <summary>
    /// Add text to OpenSearch. Returns OpenSearch (not DB) ID
    /// </summary>
    Task<string> IndexTextAsync(IndexableText textToIndex);

    #endregion
    
    #region Search

    /// <summary>
    /// Search for texts. String queries can be null if search by this query is not needed, the same for list queries, but list query
    /// must be empty in this case
    /// WARNING: We DO NOT use scroll here, hoping that take parameter is reasonably small (less than 10000 to be precise)
    /// </summary>
    /// <returns>Tuple, where Item1 is the collection of found texts, Item2 - total amount of texts, matched by query</returns>
    Task<Tuple<IReadOnlyCollection<IndexableText>, long>> SearchForTextsAsync
    (
        string titleQuery,
        string descriptionQuery,
        string contentQuery,
        string authorQuery,
        IReadOnlyCollection<string> tagsToIncludeQuery,
        IReadOnlyCollection<string> tagsToExcludeQuery,
        int skip,
        int take
    );

    /// <summary>
    /// Search for creatures. Display name query may be null, in this case ALL creatures will be returned
    /// </summary>
    Task<IReadOnlyCollection<IndexableCreature>> SearchForCreaturesAsync(string displayNameQuery);

    /// <summary>
    /// Search for tags. Tag name query may be null, in this case ALL tags will be returned
    /// </summary>
    Task<IReadOnlyCollection<IndexableTag>> SearchForTagsAsync(string tagNameQuery);

    #endregion
}