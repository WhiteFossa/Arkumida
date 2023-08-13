using System.Text;
using webapi.Dao.Abstract;
using webapi.Dao.Models.Enums.RenderedTexts;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Services.Abstract;
using webapi.Services.Abstract.TextRenderers;

namespace webapi.Services.Implementations;

public class TextsRenderingService : ITextsRenderingService
{
    private readonly ITextsDao _textsDao;
    private readonly ITextsMapper _textsMapper;
    private readonly ITextFilesMapper _textFilesMapper;
    private readonly IPlainTextRenderer _plainTextRenderer;
    private readonly ITextUtilsService _textUtilsService;

    public TextsRenderingService
    (
        ITextsDao textsDao,
        ITextsMapper textsMapper,
        ITextFilesMapper textFilesMapper,
        IPlainTextRenderer plainTextRenderer,
        ITextUtilsService textUtilsService
    )
    {
        _textsDao = textsDao;
        _textsMapper = textsMapper;
        _textFilesMapper = textFilesMapper;
        _plainTextRenderer = plainTextRenderer;
        _textUtilsService = textUtilsService;
    }
    
    public async Task<byte[]> RenderTextToFileContent(Guid textId, RenderedTextType type)
    {
        var textMetadata = _textsMapper.Map(await _textsDao.GetTextMetadataByIdAsync(textId));
        
        var rawText = await _textUtilsService.GetRawTextAsync(textId);
        var textFiles = _textFilesMapper.Map(await _textsDao.GetTextFilesByTextAsync(textId));
        
        var parsedText = _textUtilsService.ParseTextToElements(rawText, textFiles);

        switch (type)
        {
            case RenderedTextType.PlainText:
                // Plain text
                return await RenderToPlainText(textMetadata, parsedText);
            
            default:
                throw new ArgumentException($"Unknown file type: {type}", nameof(type));
        }
    }
    
    private async Task<byte[]> RenderToPlainText(Text textMetadata, IReadOnlyCollection<TextElementDto> textElements)
    {
        var renderedText = await _plainTextRenderer.RenderAsync(textMetadata, textElements);

        return Encoding.UTF8.GetBytes(renderedText);
    }
}