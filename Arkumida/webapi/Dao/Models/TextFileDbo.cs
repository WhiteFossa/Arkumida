using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

/// <summary>
/// File, attached to text
/// </summary>
public class TextFileDbo
{
    /// <summary>
    /// Text file ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// File name as it appear in text
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// File
    /// </summary>
    public FileDbo File { get; set; }
}