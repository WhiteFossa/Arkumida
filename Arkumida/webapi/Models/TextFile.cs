namespace webapi.Models;

public class TextFile
{
    /// <summary>
    /// Text file ID
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// File name as it appear in text
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// File
    /// </summary>
    public File File { get; set; }
}