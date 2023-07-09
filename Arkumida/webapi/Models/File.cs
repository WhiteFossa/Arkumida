namespace webapi.Models;

/// <summary>
/// File (as for download)
/// </summary>
public class File
{
    /// <summary>
    /// File content
    /// </summary>
    public byte[] Content { get; private set; }

    /// <summary>
    /// Furry-readable name (with extension)
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// File type
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    /// Last modified date
    /// </summary>
    public DateTime LastModified { get; private set; }

    /// <summary>
    /// SHA-512 hash of file, to use as ETag
    /// </summary>
    public string Hash { get; private set; }

    public File(byte[] content, string type, string name, DateTime lastModified, string hash)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException(nameof(name));
        }

        if (string.IsNullOrEmpty(hash))
        {
            throw new ArgumentException(nameof(hash));
        }

        Content = content ?? throw new ArgumentNullException(nameof(content));

        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("File type must be specified!", nameof(type));
        }
        Type = type;

        Name = name;
        LastModified = lastModified;
        Hash = hash;
    }
}