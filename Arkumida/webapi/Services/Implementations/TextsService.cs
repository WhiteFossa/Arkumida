using webapi.Dao.Abstract;
using webapi.Dao.Models.Enums;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Enums;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class TextsService : ITextsService
{
    private readonly ITextsDao _textsDao;
    private readonly ITextsMapper _textsMapper;

    public TextsService
    (
        ITextsDao textsDao,
        ITextsMapper textsMapper
    )
    {
        _textsDao = textsDao;
        _textsMapper = textsMapper;
    }
    
    public async Task CreateTextAsync(Text text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text), "Text mustn't be null.");

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
            .Select(tm => new TextInfoDto
            (
                tm.Id,
                "not_ready",
                new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
                new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
                new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
                tm.Title,
                tm.CreateTime,
                0,
                0,
                0,
                0,
                new List<TextTagDto>(),
                TextType.Story,
                SpecialTextType.Normal,
                new List<TextIconDto>(),
                new List<TextIconDto>(),
                tm.Description,
                10000,
                3
            ))
            .ToList();
    }

    public async Task<TextInfoDto> GetTextMetadataByIdAsync(Guid textId)
    {
        var textMetadata = await _textsDao.GetTextMetadataByIdAsync(textId);

        return new TextInfoDto
        (
            textMetadata.Id,
            "not_ready",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            textMetadata.Title,
            textMetadata.CreateTime,
            0,
            0,
            0,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            textMetadata.Description,
            10000,
            3
        );
    }

    public async Task<int> GetTotalTextsCountAsync()
    {
        return await _textsDao.GetTotalTextsCountAsync();
    }
}