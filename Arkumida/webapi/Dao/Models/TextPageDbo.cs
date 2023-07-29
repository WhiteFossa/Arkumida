using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

/// <summary>
/// Text page. Text consist of pages, pages contains sections
/// </summary>
public class TextPageDbo
{
    /// <summary>
    /// Page ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Page number, order pages by this value
    /// </summary>
    public int Number { get; set; }
    
    /// <summary>
    /// Text sections
    /// </summary>
    public IList<TextSectionDbo> Sections { get; set; }
}