using webapi.Models;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with texts sections variants
/// </summary>
public interface ITextsSectionsVariantsService
{
    Task CreateVariantAsync(TextSectionVariant variant);
}