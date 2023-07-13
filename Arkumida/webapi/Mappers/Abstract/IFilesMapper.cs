using webapi.Dao.Models;
using webapi.Models;
using File = webapi.Models.File;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for files
/// </summary>
public interface IFilesMapper
{
    IReadOnlyCollection<File> Map(IEnumerable<FileDbo> files);

    File Map(FileDbo file);

    FileDbo Map(File file);

    IReadOnlyCollection<FileDbo> Map(IEnumerable<File> files);
}