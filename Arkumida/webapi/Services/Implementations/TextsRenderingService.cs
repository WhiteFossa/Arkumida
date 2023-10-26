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
    private readonly IRawTextRenderer _rawTextRenderer;

    public TextsRenderingService
    (
        ITextsDao textsDao,
        ITextsMapper textsMapper,
        ITextFilesMapper textFilesMapper,
        IPlainTextRenderer plainTextRenderer,
        ITextUtilsService textUtilsService,
        IRenderedTextsDao renderedTextsDao,
        IRenderedTextsMapper renderedTextsMapper,
        IFilesDao filesDao,
        IRawTextRenderer rawTextRenderer
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
        _rawTextRenderer = rawTextRenderer;
    }
    
    public async Task<byte[]> RenderTextToFileContentAsync(Text metadata, RenderedTextType type)
    {
        var rawText = await _textUtilsService.GetRawTextAsync(metadata.Id);
        var textFiles = _textFilesMapper.Map(await _textsDao.GetTextFilesByTextAsync(metadata.Id));
        
        var parsedText = _textUtilsService.ParseTextToElements(rawText, textFiles);

        switch (type)
        {
            case RenderedTextType.PlainText:
                // Plain text
                return await RenderToPlainText(metadata, parsedText);
            
            default:
                throw new ArgumentException($"Unknown file type: {type}", nameof(type));
        }
    }

    public async Task<string> RenderTextContentToString(Text metadata)
    {
        var rawText = await _textUtilsService.GetRawTextAsync(metadata.Id);
        var textFiles = _textFilesMapper.Map(await _textsDao.GetTextFilesByTextAsync(metadata.Id));
        
        var parsedText = _textUtilsService.ParseTextToElements(rawText, textFiles);

        return await _rawTextRenderer.RenderAsync(metadata, parsedText);
    }

    public async Task<RenderedText> GetRenderedTextAsync(Guid textId, RenderedTextType type)
    {
        return _renderedTextsMapper.Map(await _renderedTextsDao.GetRenderedTextAsync(textId, type));
    }

    public async Task<RenderedText> RenderTextToDbAsync(Guid textId, RenderedTextType type)
    {
        var textMetadata = await _textUtilsService.GetTextMetadataAsync(textId);
        
        var renderedContent = await RenderTextToFileContentAsync(textMetadata, type);

        // Storing file in DB
        string fileType;
        string fileName = GenerateFilenameFormMetadata(textMetadata);
        
        switch (type)
        {
            case RenderedTextType.PlainText:
                fileType = "text/plain";
                fileName = $"{fileName}.txt";
                break;
            
            default:
                throw new ArgumentException($"Unknown file type: {type}", nameof(type));
        }
        
        var renderedTextFile = new FileDbo()
        {
            Content = renderedContent,
            Name = fileName,
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

    /// <summary>
    /// Generate filename (for rendered text file). ONLY FILENAME, WITHOUT EXTENSION
    /// </summary>
    private string GenerateFilenameFormMetadata(Text text)
    {
        var authors = string.Join(", ", text.Authors.Select(a => a.DisplayName));
        
        return FilesHelper.EscapeFilename($"{authors} - {text.Title}");
    }
    
    private async Task<byte[]> RenderToPlainText(Text textMetadata, IReadOnlyCollection<TextElementDto> textElements)
    {
        var renderedText = await _plainTextRenderer.RenderAsync(textMetadata, textElements);

        return Encoding.UTF8.GetBytes(renderedText);
    }
}