using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class RenderedTextsMapper : IRenderedTextsMapper
{
    private readonly ITextsMapper _textsMapper;
    private readonly IFilesMapper _filesMapper;

    public RenderedTextsMapper
    (
        ITextsMapper textsMapper,
        IFilesMapper filesMapper
    )
    {
        _textsMapper = textsMapper;
        _filesMapper = filesMapper;
    }
    
    public IReadOnlyCollection<RenderedText> Map(IEnumerable<RenderedTextDbo> renderedTexts)
    {
        if (renderedTexts == null)
        {
            return null;
        }

        return renderedTexts.Select(rt => Map(rt)).ToList();
    }

    public RenderedText Map(RenderedTextDbo renderedText)
    {
        if (renderedText == null)
        {
            return null;
        }

        return new RenderedText()
        {
            Id = renderedText.Id,
            Type = renderedText.Type,
            Text = _textsMapper.Map(renderedText.Text),
            File = _filesMapper.Map(renderedText.File)
        };
    }

    public RenderedTextDbo Map(RenderedText renderedText)
    {
        if (renderedText == null)
        {
            return null;
        }

        return new RenderedTextDbo()
        {
            Id = renderedText.Id,
            Type = renderedText.Type,
            Text = _textsMapper.Map(renderedText.Text),
            File = _filesMapper.Map(renderedText.File)
        };
    }

    public IReadOnlyCollection<RenderedTextDbo> Map(IEnumerable<RenderedText> renderedTexts)
    {
        if (renderedTexts == null)
        {
            return null;
        }

        return renderedTexts.Select(rt => Map(rt)).ToList();
    }
}