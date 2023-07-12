using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;
using File = webapi.Models.File;

namespace webapi.Mappers.Implementations;

public class FilesMapper : IFilesMapper
{
    public IReadOnlyCollection<File> Map(IEnumerable<FileDbo> files)
    {
        if (files == null)
        {
            return null;
        }

        return files.Select(f => Map(f)).ToList();
    }

    public File Map(FileDbo file)
    {
        if (file == null)
        {
            return null;
        }

        return new File()
        {
            Id = file.Id,
            Content = file.Content,
            Hash = file.Hash,
            Name = file.Name,
            Type = file.Type,
            LastModifiedTime = file.LastModifiedTime
        };
    }

    public FileDbo Map(File file)
    {
        if (file == null)
        {
            return null;
        }

        return new FileDbo()
        {
            Id = file.Id,
            Content = file.Content,
            Hash = file.Hash,
            Name = file.Name,
            Type = file.Type,
            LastModifiedTime = file.LastModifiedTime
        };
    }

    public IReadOnlyCollection<FileDbo> Map(IEnumerable<File> files)
    {
        if (files == null)
        {
            return null;
        }

        return files.Select(f => Map(f)).ToList();
    }
}