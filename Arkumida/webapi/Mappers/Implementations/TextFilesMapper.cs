using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class TextFilesMapper : ITextFilesMapper
{
    private readonly IFilesMapper _filesMapper;

    public TextFilesMapper(IFilesMapper filesMapper)
    {
        _filesMapper = filesMapper;
    }
    
    public IReadOnlyCollection<TextFile> Map(IEnumerable<TextFileDbo> textFiles)
    {
        if (textFiles == null)
        {
            return null;
        }

        return textFiles.Select(tf => Map(tf)).ToList();
    }

    public TextFile Map(TextFileDbo textFile)
    {
        return new TextFile()
        {
            Id = textFile.Id,
            Name = textFile.Name,
            File = _filesMapper.Map(textFile.File)
        };
    }

    public TextFileDbo Map(TextFile textFile)
    {
        return new TextFileDbo()
        {
            Id = textFile.Id,
            Name = textFile.Name,
            File = _filesMapper.Map(textFile.File)
        };
    }

    public IReadOnlyCollection<TextFileDbo> Map(IEnumerable<TextFile> textFiles)
    {
        if (textFiles == null)
        {
            return null;
        }

        return textFiles.Select(tf => Map(tf)).ToList();
    }
}