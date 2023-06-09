using webapi.Models.Api.DTOs;

namespace webapi.Models;

/// <summary>
/// Tag (can be used as a text tag and as a category)
/// </summary>
public class Tag : IdedEntity
{
    /// <summary>
    /// Tag name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// If true, than this tag represents a category
    /// </summary>
    public bool IsCategory { get; set; }

    /// <summary>
    /// If tag is category, then order it by this value
    /// </summary>
    public int CategoryOrder { get; set; }

    /// <summary>
    /// How much texts are marked with this tag
    /// </summary>
    public int TextsCount { get; set; }

    /// <summary>
    /// Generate TextTagDto from tag
    /// </summary>
    public TextTagDto ToTextTagDto()
    {
        return new TextTagDto(Id, FurryReadableId, Name);
    }

    /// <summary>
    /// Generate CategoryTagDto from tag
    /// </summary>
    public CategoryTagDto ToCategoryTagDto()
    {
        if (!IsCategory)
        {
            throw new InvalidOperationException($"Tag with ID={Id} is not a category tag.");
        }
        
        return new CategoryTagDto(Id, FurryReadableId, Name, TextsCount);
    }
}