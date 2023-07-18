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

namespace webapi.Services.Implementations;

public class TextsService : ITextsService
{
    private const int ParserFastSkipTextLength = 1; // We are searching for one character - "["
    
    private readonly ITextsDao _textsDao;
    private readonly ITextsMapper _textsMapper;
    private readonly ITagsMapper _tagsMapper;
    private readonly ITagsService _tagsService;
    private readonly ITextsSectionsMapper _textsSectionsMapper;
    private readonly ITextFilesMapper _textFilesMapper;

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
        new ParserEmbeddedImage()
    };

    public TextsService
    (
        ITextsDao textsDao,
        ITextsMapper textsMapper,
        ITagsMapper tagsMapper,
        ITagsService tagsService,
        ITextsSectionsMapper textsSectionsMapper,
        ITextFilesMapper textFilesMapper
    )
    {
        _textsDao = textsDao;
        _textsMapper = textsMapper;
        _tagsMapper = tagsMapper;
        _tagsService = tagsService;
        _textsSectionsMapper = textsSectionsMapper;
        _textFilesMapper = textFilesMapper;
    }

    public async Task CreateTextAsync(Text text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text), "Text mustn't be null.");

        text.TextFiles = new List<TextFile>(); // We have no files when creating text, they will be (if any) attached later
        
        var dbText = _textsMapper.Map(text);
        dbText.Id = Guid.Empty;

        await _textsDao.CreateTextAsync(dbText);

        text.Id = dbText.Id;
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

        return textsMetadata
            .Select(tm =>
            {
                var tags = _tagsMapper.Map(tm.Tags);
                
                return new TextInfoDto
                (
                    tm.Id,
                    "not_ready",
                    new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
                    new CreatureDto(new Guid("15829718-169d-4933-b794-efef888df717"), "2", "Редгерра"),
                    new CreatureDto(new Guid("86938a87-d2d8-471b-8d7a-ffba4b89a7f8"), "3", "Ааз"),
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
                    3,
                    tm.IsIncomplete
                );
            })
            .ToList();
    }

    public async Task<TextInfoDto> GetTextMetadataByIdAsync(Guid textId)
    {
        var textMetadata = await _textsDao.GetTextMetadataByIdAsync(textId);

        var textTags = _tagsMapper.Map(textMetadata.Tags);

        return new TextInfoDto
        (
            textMetadata.Id,
            "not_ready",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            new CreatureDto(new Guid("15829718-169d-4933-b794-efef888df717"), "2", "Редгерра"),
            new CreatureDto(new Guid("86938a87-d2d8-471b-8d7a-ffba4b89a7f8"), "3", "Ааз"),
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
            3,
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
        var textData = await _textsDao.GetTextByIdAsync(textId);
        
        var textTags = _tagsMapper.Map(textData.Tags);
        var textFiles = _textFilesMapper.Map(textData.TextFiles);

        return new TextReadDto
        (
            textData.Id,
            "not_ready",
            textData.CreateTime,
            textData.LastUpdateTime,
            textData.Title,
            textData.Description,
            OrderTextSections(_textsSectionsMapper.Map(textData.Sections))
                .Select(ts => ts.ToDto(textFiles, this))
                .ToList(),
            _tagsService.OrderTags(textTags).Select(t => t.ToTagDto()).ToList(),
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            new CreatureDto(new Guid("15829718-169d-4933-b794-efef888df717"), "2", "Редгерра"),
            new CreatureDto(new Guid("86938a87-d2d8-471b-8d7a-ffba4b89a7f8"), "3", "Ааз")
        );
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