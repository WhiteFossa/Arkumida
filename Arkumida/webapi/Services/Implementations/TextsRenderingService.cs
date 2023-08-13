using System.Text;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums.RenderedTexts;
using webapi.Helpers;
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
    private readonly IRenderedTextsDao _renderedTextsDao;
    private readonly IRenderedTextsMapper _renderedTextsMapper;
    private readonly IFilesDao _filesDao;

    public TextsRenderingService
    (
        ITextsDao textsDao,
        ITextsMapper textsMapper,
        ITextFilesMapper textFilesMapper,
        IPlainTextRenderer plainTextRenderer,
        ITextUtilsService textUtilsService,
        IRenderedTextsDao renderedTextsDao,
        IRenderedTextsMapper renderedTextsMapper,
        IFilesDao filesDao
    )
    {
        _textsDao = textsDao;
        _textsMapper = textsMapper;
        _textFilesMapper = textFilesMapper;
        _plainTextRenderer = plainTextRenderer;
        _textUtilsService = textUtilsService;
        _renderedTextsDao = renderedTextsDao;
        _renderedTextsMapper = renderedTextsMapper;
        _filesDao = filesDao;
    }
    
    public async Task<byte[]> RenderTextToFileContentAsync(Guid textId, RenderedTextType type)
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

    public async Task<RenderedText> GetRenderedTextAsync(Guid textId, RenderedTextType type)
    {
        return _renderedTextsMapper.Map(await _renderedTextsDao.GetRenderedTextAsync(textId, type));
    }

    public async Task<RenderedText> RenderTextToDbAsync(Guid textId, RenderedTextType type)
    {
        var textMetadata = _textsMapper.Map(await _textsDao.GetTextMetadataByIdAsync(textId));
        
        var renderedContent = await RenderTextToFileContentAsync(textId, type);

        // Storing file in DB
        string fileType;
        switch (type)
        {
            case RenderedTextType.PlainText:
                fileType = "text/plain";
                break;
            
            default:
                throw new ArgumentException($"Unknown file type: {type}", nameof(type));
        }
        
        var renderedTextFile = new FileDbo()
        {
            Content = renderedContent,
            Name = textMetadata.Title, // TODO: Remove spaces and so on?
            Type = fileType,
            LastModifiedTime = DateTime.UtcNow,
            Hash = SHA512Helper.CalculateSHA512(renderedContent)
        };

        await _filesDao.CreateFileAsync(renderedTextFile);
        
        // Creating rendered text
        var renderedText = new RenderedTextDbo()
        {
            Type = type,
            File = renderedTextFile,
            Text = _textsMapper.Map(textMetadata)
        };

        await _renderedTextsDao.CreateRenderedTextAsync(renderedText);
        
        return _renderedTextsMapper.Map(renderedText);
    }

    public async Task<RenderedText> GetAndRenderIfNotExistAsync(Guid textId, RenderedTextType type)
    {
        var renderedText = _renderedTextsMapper.Map(await _renderedTextsDao.GetRenderedTextAsync(textId, type));

        if (renderedText == null)
        {
            renderedText = await RenderTextToDbAsync(textId, type);
        }

        return renderedText;
    }

    private async Task<byte[]> RenderToPlainText(Text textMetadata, IReadOnlyCollection<TextElementDto> textElements)
    {
        var renderedText = await _plainTextRenderer.RenderAsync(textMetadata, textElements);

        return Encoding.UTF8.GetBytes(renderedText);
    }
}