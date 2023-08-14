using webapi.Dao.Models;
using webapi.Models;
using File = System.IO.File;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Rendered texts mapper
/// </summary>
public interface IRenderedTextsMapper
{
    IReadOnlyCollection<RenderedText> Map(IEnumerable<RenderedTextDbo> renderedTexts);

    RenderedText Map(RenderedTextDbo renderedText);

    RenderedTextDbo Map(RenderedText renderedText);

    IReadOnlyCollection<RenderedTextDbo> Map(IEnumerable<RenderedText> renderedTexts);
}