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

public class TextUtilsService : ITextUtilsService
{
    private const int ParserFastSkipTextLength = 1; // We are searching for one character - "["

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

    private readonly ITextsDao _textsDao;
    private readonly IAccountsService _accountsService;
    private readonly ITextsMapper _textsMapper;

    public TextUtilsService
    (
        ITextsDao textsDao,
        IAccountsService accountsService,
        ITextsMapper textsMapper
    )
    {
        _textsDao = textsDao;
        _accountsService = accountsService;
        _textsMapper = textsMapper;
    }

    public async Task<string> GetRawTextAsync(Guid textId)
    {
        var pages = (await _textsDao.GetAllPagesAsync(textId))
            .OrderBy(p => p.Number);

        var rawTextSb = new StringBuilder();

        foreach (var page in pages)
        {
            foreach (var section in page.Sections.OrderBy(s => s.Order))
            {
                var lastVariant = section.Variants
                    .OrderByDescending(v => v.CreationTime)
                    .First();

                rawTextSb.Append(lastVariant.Content);
            }
        }

        return rawTextSb.ToString();
    }

    public IReadOnlyCollection<TextElementDto> ParseTextToElements(string textContent, IReadOnlyCollection<TextFile> textFiles)
    {
        var result = new List<TextElementDto>();

        result.Add(new TextElementDto(TextElementType.ParagraphBegin, "", new string[] { }));

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

        result.Add(new TextElementDto(TextElementType.PlainText, currentTextSb.ToString(), new string[] { }));
        result.Add(new TextElementDto(TextElementType.ParagraphEnd, "", new string[] { }));

        return result;
    }

    public async Task<Text> GetTextMetadataAsync(Guid textId)
    {
        return await PopulateTextMetadata(await _textsDao.GetTextMetadataByIdAsync(textId));
    }

    public async Task<IReadOnlyCollection<Text>> GetTextsMetadatasAsync(TextOrderMode orderMode, int skip, int take)
    {
        var metadatas = await _textsDao.GetTextsMetadataAsync(orderMode, skip, take);

        return metadatas
            .Select(async metadata => await PopulateTextMetadata(metadata))
            .Select(t => t.Result)
            .ToList();
    }

    /// <summary>
    /// Loads missing data into the text
    /// </summary>
    private async Task<Text> PopulateTextMetadata(TextDbo metadata)
    {
        var text = _textsMapper.Map(metadata);

        text.Publisher = await _accountsService.GetProfileByCreatureIdAsync(metadata.Publisher.Id);

        text.Authors = metadata
            .Authors
            .Select(async a => await _accountsService.GetProfileByCreatureIdAsync(a.Id))
            .Select(t => t.Result)
            .ToList();

        text.Translators = metadata
            .Translators
            .Select(async t => await _accountsService.GetProfileByCreatureIdAsync(t.Id))
            .Select(t => t.Result)
            .ToList();

        return text;
    }
}