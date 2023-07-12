using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for files, embedded into texts
/// </summary>
public interface ITextFilesMapper
{
    IReadOnlyCollection<TextFile> Map(IEnumerable<TextFileDbo> textFiles);

    TextFile Map(TextFileDbo textFile);

    TextFileDbo Map(TextFile textFile);

    IReadOnlyCollection<TextFileDbo> Map(IEnumerable<TextFile> textFiles);
}