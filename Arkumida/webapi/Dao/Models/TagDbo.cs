using System.ComponentModel.DataAnnotations;
using webapi.Dao.Models.Enums;

namespace webapi.Dao.Models;

/// <summary>
/// Tag, database object
/// </summary>
public class TagDbo
{
    /// <summary>
    /// Tag ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Furry readable ID
    /// </summary>
    public string FurryReadableId { get; set; }

    /// <summary>
    /// Tag subtype
    /// </summary>
    public TagSubtype Subtype { get; set; }

    /// <summary>
    /// Tag name, i.e. tag itself
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// If true, then this tag is category
    /// </summary>
    public bool IsCategory { get; set; }

    /// <summary>
    /// If tag is category, then order it by this value
    /// </summary>
    public int CategoryOrder { get; set; }

    /// <summary>
    /// Category type
    /// </summary>
    public CategoryTagType CategoryType { get; set; }
}