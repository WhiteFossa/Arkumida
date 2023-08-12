using System.Text;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Models.ParserTags;
using webapi.Services.Abstract;
using webapi.Services.Abstract.TextRenderers;

namespace webapi.Services.Implementations;

public class TextsService : ITextsService
{
    private const int ParserFastSkipTextLength = 1; // We are searching for one character - "["
    
    private readonly ITextsDao _textsDao;
    private readonly ITextsMapper _textsMapper;
    private readonly ITagsMapper _tagsMapper;
    private readonly ITagsService _tagsService;
    private readonly ITextsPagesMapper _textsPagesMapper;
    private readonly ITextFilesMapper _textFilesMapper;
    private readonly ICreaturesMapper _creaturesMapper;
    private readonly IPlainTextRenderer _plainTextRenderer;

    private readonly IReadOnlyCollection<ParserTagBase> _parserTags = new List<ParserTagBase>()
    {
        new ParserParagraph(),
        new ParserFullWidthAlignedTextBegin(),
        new ParserFullWidthAlignedTextEnd(),
        new ParserItalicTextBegin(),
        new ParserItalicTextEnd(),
        new ParserBoldTextBegin(),
        new ParserBoldTextEnd(),
        new ParserUnderlineTextBegin(),
        new ParserUnderlineTextEnd(),
        new ParserStrikeOutTextBegin(),
        new ParserStrikeOutTextEnd(),
        new ParserCentrallyAlignedTextBegin(),
        new ParserCentrallyAlignedTextEnd(),
        new ParserLeftAlignedTextBegin(),
        new ParserLeftAlignedTextEnd(),
        new ParserRightAlignedTextBegin(),
        new ParserRightAlignedTextEnd(),
        new ParserTitleBegin(),
        new ParserTitleEnd(),
        new ParserPreformattedTextBegin(),
        new ParserPreformattedTextEnd(),
        new ParserQuoteBegin(),
        new ParserQuoteEnd(),
        new ParserAsciiArtBegin(),
        new ParserAsciiArtEnd(),
        new ParserUrl(),
        new ParserColor(),
        new ParserHrefedUrl(),
        new ParserSizedAsciiArt(),
        new ParserEmbeddedImage(),
        new ParserComicsImage()
    };

    public TextsService
    (
        ITextsDao textsDao,
        ITextsMapper textsMapper,
        ITagsMapper tagsMapper,
        ITagsService tagsService,
        ITextsPagesMapper textsPagesMapper,
        ITextFilesMapper textFilesMapper,
        ICreaturesMapper creaturesMapper,
        IPlainTextRenderer plainTextRenderer
    )
    {
        _textsDao = textsDao;
        _textsMapper = textsMapper;
        _tagsMapper = tagsMapper;
        _tagsService = tagsService;
        _textsPagesMapper = textsPagesMapper;
        _textFilesMapper = textFilesMapper;
        _creaturesMapper = creaturesMapper;
        _plainTextRenderer = plainTextRenderer;
    }

    public async Task<Text> CreateTextAsync(Text text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text), "Text mustn't be null.");

        text.TextFiles = new List<TextFile>(); // We have no files when creating text, they will be (if any) attached later
        
        var dbText = _textsMapper.Map(text);
        dbText.Id = Guid.Empty;

        await _textsDao.CreateTextAsync(dbText);

        return _textsMapper.Map(dbText); // To have ID and other fields (like Publisher) populated 
    }

    public async Task<IReadOnlyCollection<TextInfoDto>> GetTextsMetadataAsync(TextOrderMode orderMode, int skip, int take)
    {
        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), "Skip must not be negative.");
        }

        if (take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(take), "Take must be positive.");
        }

        var textsMetadata = await _textsDao.GetTextsMetadataAsync(orderMode, skip, take);

        var textsIds = textsMetadata
            .Select(tm => tm.Id)
            .ToList();
        
        var sizesInPages = await _textsDao.GetPagesCountByTexts(textsIds);

        return textsMetadata
            .Select(tm =>
            {
                var tags = _tagsMapper.Map(tm.Tags);
                
                return new TextInfoDto
                (
                    tm.Id,
                    "not_ready",
                    _creaturesMapper.Map(tm.Authors).Select(ta => ta.ToDto()).ToList(),
                    _creaturesMapper.Map(tm.Translators).Select(tt => tt.ToDto()).ToList(),
                    _creaturesMapper.Map(tm.Publisher).ToDto(),
                    tm.Title,
                    tm.CreateTime,
                    tm.ReadsCount,
                    0,
                    tm.VotesPlus,
                    tm.VotesMinus,
                    _tagsService.OrderTags(tags)
                        .Select(t => t.ToTextTagDto())
                        .ToList(),
                    new List<TextIconDto>(),
                    AddIllustrationsIconToRightIcons(new List<TextIconDto>(), tm),
                    tm.Description,
                    10000,
                    sizesInPages[tm.Id],
                    tm.IsIncomplete
                );
            })
            .ToList();
    }

    public async Task<TextInfoDto> GetTextMetadataByIdAsync(Guid textId)
    {
        var textMetadata = await _textsDao.GetTextMetadataByIdAsync(textId);

        var textTags = _tagsMapper.Map(textMetadata.Tags);

        var sizeInPages = (await _textsDao.GetPagesCountByTexts(new List<Guid>() { textId }))
            .Single()
            .Value;

        return new TextInfoDto
        (
            textMetadata.Id,
            "not_ready",
            _creaturesMapper.Map(textMetadata.Authors).Select(ta => ta.ToDto()).ToList(),
            _creaturesMapper.Map(textMetadata.Translators).Select(tt => tt.ToDto()).ToList(),
            _creaturesMapper.Map(textMetadata.Publisher).ToDto(),
            textMetadata.Title,
            textMetadata.CreateTime,
            textMetadata.ReadsCount,
            0,
            textMetadata.VotesPlus,
            textMetadata.VotesMinus,
            _tagsService
                .OrderTags(textTags)
                .Select(t => t.ToTextTagDto())
                .ToList(),
            new List<TextIconDto>(),
            AddIllustrationsIconToRightIcons(new List<TextIconDto>(), textMetadata),
            textMetadata.Description,
            10000,
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
        var textMetadata = await _textsDao.GetTextMetadataByIdAsync(textId);
        
        var textTags = _tagsMapper.Map(textMetadata.Tags);
        
        var textFiles = _textFilesMapper.Map(await _textsDao.GetTextFilesByTextAsync(textId));
        
        var sizeInPages = (await _textsDao.GetPagesCountByTexts(new List<Guid>() { textId }))
            .Single()
            .Value;

        // TODO: Debug code, remove me
        var text = _textsMapper.Map(textMetadata);
        var rawText = await GetRawTextAsync(textId);
        var parsedText = ParseTextToElements(rawText, textFiles);
        var plainText = await _plainTextRenderer.RenderAsync(text, parsedText);

        return new TextReadDto
        (
            textMetadata.Id,
            "not_ready",
            textMetadata.CreateTime,
            textMetadata.LastUpdateTime,
            textMetadata.Title,
            textMetadata.Description,
            _tagsService.OrderTags(textTags)
                .Select(t => t.ToTagDto())
                .ToList(),
            _creaturesMapper.Map(textMetadata.Authors).Select(ta => ta.ToDto()).ToList(),
            _creaturesMapper.Map(textMetadata.Translators).Select(tt => tt.ToDto()).ToList(),
            _creaturesMapper.Map(textMetadata.Publisher).ToDto(),
            textFiles
                .Select(tf => new TextFileDto(tf.Id, tf.Name, new FileInfoDto(tf.File.Id, tf.File.Name)))
                .ToList(),
            sizeInPages
        );
    }

    public async Task<TextPageDto> GetTextPageAsync(Guid textId, int pageNumber)
    {
        // TODO: Parallelize me
        var pageData = _textsPagesMapper.Map(await _textsDao.GetPageAsync(textId, pageNumber));
        var textFiles = _textFilesMapper.Map(await _textsDao.GetTextFilesByTextAsync(textId));

        pageData.Sections = OrderTextSections(pageData.Sections).ToList();

        return pageData.ToDto(textFiles, this);
    }

    public IReadOnlyCollection<TextSection> OrderTextSections(IEnumerable<TextSection> sections)
    {
        return sections
            .OrderBy(s => s.Order)
            .ToList();
    }

    public IReadOnlyCollection<TextElementDto> ParseTextToElements(string textContent, IReadOnlyCollection<TextFile> textFiles)
    {
        var result = new List<TextElementDto>();

        result.Add(new TextElementDto(TextElementType.ParagraphBegin, "", new string[] {}));

        var currentTextSb = new StringBuilder();
        for (var charIndex = 0; charIndex < textContent.Length; charIndex++)
        {
            var remaining = textContent.Length - charIndex;
            
            var fastSkipText = textContent.Substring(charIndex, Math.Min(ParserFastSkipTextLength, remaining));
            
            // Trying to match tags
            var isMatched = false;
            foreach (var tag in _parserTags)
            {
                // Fast skip
                if (tag.IsFastSkip(fastSkipText))
                {
                    continue;
                }
                
                // Full analysis
                var matchResult = tag.TryMatch(textContent.Substring(charIndex, Math.Min(tag.GetRequestedTextLength(), remaining)));
                
                if (matchResult.Item1)
                {
                    // We have a match
                    tag.Action(result, currentTextSb.ToString(), matchResult.Item3, textFiles);
                    currentTextSb.Clear();
                    
                    charIndex += matchResult.Item2 - 1;
                    remaining -= matchResult.Item2 - 1;
                    
                    isMatched = true;
                }
            }

            if (!isMatched)
            {
                // Ordinary character
                currentTextSb.Append(textContent.Substring(charIndex, 1));
            }
        }

        result.Add(new TextElementDto(TextElementType.PlainText, currentTextSb.ToString(), new string[] {}));
        result.Add(new TextElementDto(TextElementType.ParagraphEnd, "", new string[] {}));

        return result;
    }

    public async Task AddFileToTextAsync(Guid textId, string fileName, Guid existingFileId)
    {
        await _textsDao.AddFileToTextAsync(textId, fileName, existingFileId);
    }

    public async Task<string> GetRawTextAsync(Guid textId)
    {
        var textMetadata = await _textsDao.GetTextMetadataByIdAsync(textId);
        var pages = (await _textsDao.GetAllPagesAsync(textId))
            .OrderBy(p => p.Number);

        var rawTextSb = new StringBuilder();

        foreach (var page in pages)
        {
            foreach (var section in page.Sections.OrderBy(s => s.Order))
            {
                var lastVariant = section.
                    Variants
                    .OrderByDescending(v => v.CreationTime)
                    .First();

                rawTextSb.Append(lastVariant.Content);
            }
        }
        
        return rawTextSb.ToString();
    }

    public async Task<string> RenderToPlainTextAsync(Guid textId)
    {
        throw new NotImplementedException();
    }

    private List<TextIconDto> AddIllustrationsIconToRightIcons(List<TextIconDto> rightIcons, TextDbo textMetadata)
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