using webapi.Dao.Abstract;
using webapi.Dao.Models.Enums;
using webapi.Dao.Models.Enums.RenderedTexts;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.OpenSearch.Models;
using webapi.OpenSearch.Services.Abstract;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class TextsService : ITextsService
{
    private readonly ITextsDao _textsDao;
    private readonly ITextsMapper _textsMapper;
    private readonly ITagsService _tagsService;
    private readonly ITextsPagesMapper _textsPagesMapper;
    private readonly ITextFilesMapper _textFilesMapper;
    private readonly ITextsRenderingService _textsRenderingService;
    private readonly ITextUtilsService _textUtilsService;
    private readonly IArkumidaOpenSearchClient _arkumidaOpenSearchClient;

    public TextsService
    (
        ITextsDao textsDao,
        ITextsMapper textsMapper,
        ITagsService tagsService,
        ITextsPagesMapper textsPagesMapper,
        ITextFilesMapper textFilesMapper,
        ITextsRenderingService textsRenderingService,
        ITextUtilsService textUtilsService,
        IArkumidaOpenSearchClient arkumidaOpenSearchClient
    )
    {
        _textsDao = textsDao;
        _textsMapper = textsMapper;
        _tagsService = tagsService;
        _textsPagesMapper = textsPagesMapper;
        _textFilesMapper = textFilesMapper;
        _textsRenderingService = textsRenderingService;
        _textUtilsService = textUtilsService;
        _arkumidaOpenSearchClient = arkumidaOpenSearchClient;
    }

    public async Task<Text> CreateTextAsync(Text text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text), "Text mustn't be null.");

        text.TextFiles = new List<TextFile>(); // We have no files when creating text, they will be (if any) attached later
        
        var dbText = _textsMapper.Map(text);
        dbText.Id = Guid.Empty;

        await _textsDao.CreateTextAsync(dbText);

        // Rendering text in various formats
        await _textsRenderingService.RenderTextToDbAsync(dbText.Id, RenderedTextType.PlainText);

        var textMetadata = await _textUtilsService.GetTextMetadataAsync(dbText.Id); 
        
        // Indexing text to OpenSearch
        var textToIndex = new IndexableText()
        {
            DbId = textMetadata.Id,
            LastUpdateTime = textMetadata.LastUpdateTime,
            Title = textMetadata.Title,
            Description = textMetadata.Description,
            Content = await _textsRenderingService.RenderTextContentToString(textMetadata),
            AuthorsDbIds = textMetadata.Authors.Select(a => a.Id).ToList(),
            TranslatorsDbIds = textMetadata.Translators.Select(t => t.Id).ToList(),
            PublisherDbId = textMetadata.Publisher.Id,
            TagsDbIds = textMetadata.Tags.Select(t => t.Id).ToList()
        };
        
        await _arkumidaOpenSearchClient.IndexTextAsync(textToIndex);
        
        return textMetadata;
    }

    public async Task<IReadOnlyCollection<TextInfoDto>> GetTextsInfosAsync(TextOrderMode orderMode, int skip, int take)
    {
        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), "Skip must not be negative.");
        }

        if (take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(take), "Take must be positive.");
        }

        var textsMetadatas = await _textUtilsService.GetTextsMetadatasAsync(orderMode, skip, take);

        var textsIds = textsMetadatas
            .Select(tm => tm.Id)
            .ToList();
        
        var sizesInPages = await _textsDao.GetPagesCountByTexts(textsIds);

        var result = new List<TextInfoDto>();

        foreach (var textMetadata in textsMetadatas)
        {
            var sizeInBytes = (await _textsRenderingService.GetRenderedTextAsync(textMetadata.Id, RenderedTextType.PlainText))
                .File
                .Content
                .Length;
            
            result.Add
            (
                new TextInfoDto
                (
                    textMetadata.Id,
                    "not_ready",
                    
                    textMetadata
                        .Authors
                        .Select(cp => cp.ToDto())
                        .ToList(),
                    
                    textMetadata
                        .Translators
                        .Select(cp => cp.ToDto())
                        .ToList(),
                    
                    textMetadata.Publisher.ToDto(),
                    
                    textMetadata.Title,
                    textMetadata.CreateTime,
                    textMetadata.ReadsCount,
                    0,
                    textMetadata.VotesPlus,
                    textMetadata.VotesMinus,
                    _tagsService.OrderTags(textMetadata.Tags)
                        .Select(t => t.ToTextTagDto())
                        .ToList(),
                    new List<TextIconDto>(),
                    AddIllustrationsIconToRightIcons(new List<TextIconDto>(), textMetadata),
                    textMetadata.Description,
                    sizeInBytes,
                    sizesInPages[textMetadata.Id],
                    textMetadata.IsIncomplete
                )
            );
        }

        return result;
    }

    public async Task<TextInfoDto> GetTextInfoByIdAsync(Guid textId)
    {
        var textMetadata = await _textUtilsService.GetTextMetadataAsync(textId);
        
        var sizeInPages = (await _textsDao.GetPagesCountByTexts(new List<Guid>() { textId }))
            .Single()
            .Value;
        
        var sizeInBytes = (await _textsRenderingService.GetRenderedTextAsync(textMetadata.Id, RenderedTextType.PlainText))
            .File
            .Content
            .Length;

        return new TextInfoDto
        (
            textMetadata.Id,
            "not_ready",
            
            textMetadata
                .Authors
                .Select(cp => cp.ToDto())
                .ToList(),
            
            textMetadata
                .Translators
                .Select(cp => cp.ToDto())
                .ToList(),
            
            textMetadata.Publisher.ToDto(),
            textMetadata.Title,
            textMetadata.CreateTime,
            textMetadata.ReadsCount,
            0,
            textMetadata.VotesPlus,
            textMetadata.VotesMinus,
            _tagsService
                .OrderTags(textMetadata.Tags)
                .Select(t => t.ToTextTagDto())
                .ToList(),
            new List<TextIconDto>(),
            AddIllustrationsIconToRightIcons(new List<TextIconDto>(), textMetadata),
            textMetadata.Description,
            sizeInBytes,
            sizeInPages,
            textMetadata.IsIncomplete
        );
    }

    public async Task<int> GetTotalTextsCountAsync()
    {
        return await _textsDao.GetTotalTextsCountAsync();
    }

    public async Task<DateTime> GetLastTextAddTimeAsync()
    {
        return await _textsDao.GetLastTextAddTimeAsync();
    }

    public async Task<TextReadDto> GetTextToReadAsync(Guid textId)
    {
        var textMetadata = await _textUtilsService.GetTextMetadataAsync(textId);
        
        var textFiles = _textFilesMapper.Map(await _textsDao.GetTextFilesByTextAsync(textId));
        
        var sizeInPages = (await _textsDao.GetPagesCountByTexts(new List<Guid>() { textId }))
            .Single()
            .Value;

        var plainTextRenderedFile = (await _textsRenderingService.GetRenderedTextAsync(textId, RenderedTextType.PlainText)).File;
        
        return new TextReadDto
        (
            textMetadata.Id,
            "not_ready",
            textMetadata.CreateTime,
            textMetadata.LastUpdateTime,
            textMetadata.Title,
            textMetadata.Description,
            
            _tagsService.OrderTags(textMetadata.Tags)
                .Select(t => t.ToTagDto())
                .ToList(),
           
            textMetadata
                .Authors
                .Select(cp => cp.ToDto())
                .ToList(),
            
            textMetadata
                .Translators
                .Select(cp => cp.ToDto())
                .ToList(),
            
            textMetadata.Publisher.ToDto(),
            
            textFiles
                .Select(tf => new TextFileDto(tf.Id, tf.Name, new FileInfoDto(tf.File.Id, tf.File.Name)))
                .ToList(),
            sizeInPages,
            new FileInfoDto(plainTextRenderedFile.Id, plainTextRenderedFile.Name)
        );
    }

    public async Task<TextPageDto> GetTextPageAsync(Guid textId, int pageNumber)
    {
        // TODO: Parallelize me
        var pageData = _textsPagesMapper.Map(await _textsDao.GetPageAsync(textId, pageNumber));
        var textFiles = _textFilesMapper.Map(await _textsDao.GetTextFilesByTextAsync(textId));

        pageData.Sections = OrderTextSections(pageData.Sections).ToList();

        return pageData.ToDto(textFiles, _textUtilsService);
    }

    public IReadOnlyCollection<TextSection> OrderTextSections(IEnumerable<TextSection> sections)
    {
        return sections
            .OrderBy(s => s.Order)
            .ToList();
    }

    public async Task AddFileToTextAsync(Guid textId, string fileName, Guid existingFileId)
    {
        await _textsDao.AddFileToTextAsync(textId, fileName, existingFileId);
    }

    private List<TextIconDto> AddIllustrationsIconToRightIcons(List<TextIconDto> rightIcons, Text textMetadata)
    {
        var result = new List<TextIconDto>();
        result.AddRange(rightIcons);
        
        if (textMetadata.TextFiles.Any())
        {
            result.Add(new TextIconDto(TextIconType.Illustrations));
        }
        
        return result;
    }
}