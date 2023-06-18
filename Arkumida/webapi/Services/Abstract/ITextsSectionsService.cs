using webapi.Models;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with texts sections
/// </summary>
public interface ITextsSectionsService
{
    /// <summary>
    /// Create new section
    /// </summary>
    Task CreateSectionAsync(TextSection section);

    /// <summary>
    /// Add existing variant to section
    /// </summary>
    Task AddVariantToSectionAsync(Guid sectionId, Guid variantId);
}