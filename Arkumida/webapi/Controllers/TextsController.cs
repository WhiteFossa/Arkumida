using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Enums;
using webapi.Models.Api.Responses;
using webapi.Services.Abstract;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with texts
/// </summary>
[ApiController]
public class TextsController : ControllerBase
{
    private readonly ITagsService _tagsService;
    
    private IList<TextInfoDto> _texts = new List<TextInfoDto>();

    public TextsController(ITagsService tagsService)
    {
        _tagsService = tagsService;
        
        _texts.Add(new TextInfoDto
        (
            new Guid("0e9fc697-31d6-4776-bf7e-05950936b4de"),
            "1",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Сказка о том, как Фосса себя за хвост укусили",
            DateTime.UtcNow,
            1337,
            27,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Snuff,
            new List<TextIconDto>(),
            new List<TextIconDto>() { new TextIconDto(TextIconType.Illustrations) },
            "Трагически-кровавая история о Фоссе и укушенном хвосте.",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("185d7cc9-83e7-4de3-b1d8-5f8d50af6a88"),
            "2",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Унылый текст, который никто не читает",
            new DateTime(2023, 06, 01, 01, 02, 03),
            3,
            0,
            0,
            10,
            new List<TextTagDto>(),
            TextType.Novel,
            SpecialTextType.Sandbox,
            new List<TextIconDto>(),
            new List<TextIconDto>() { new TextIconDto(TextIconType.Incomplete) },
            "Сплошное уныние",
            2000,
            1
        ));

        _texts.Add(new TextInfoDto
        (
            new Guid("e77c2183-2c49-4e56-a7a5-576f674f1403"),
            "3",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            new CreatureDto(new Guid("ea405a37-57b5-4e9f-bedd-8e71c9e32cf5"), "2", "Ааз"),
            new CreatureDto(new Guid("06aefc8b-2804-4057-a6fa-c8ac66e45e82"), "3", "Редгерра"),
            "Обычный поэтический гетеройифф с конкурса",
            new DateTime(2023, 06, 01, 02, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Poetry,
            SpecialTextType.Contest,
            new List<TextIconDto>() { new TextIconDto(TextIconType.MLP) },
            new List<TextIconDto>() { new TextIconDto(TextIconType.Series, "/texts/bySeries/2b1d2462-084a-4566-b9e1-c68411b5e7e0") },
            "Можно-ли поэтически писать о... понях?",
            5000,
            5
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("a4ade7ba-7c67-4ba7-abb9-06d11b98d2b6"),
            "4",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            new CreatureDto(new Guid("06aefc8b-2804-4057-a6fa-c8ac66e45e82"), "3", "Редгерра"),
            new CreatureDto(new Guid("ea405a37-57b5-4e9f-bedd-8e71c9e32cf5"), "2", "Ааз"),
            "Ничем непримечательный гомойифф",
            new DateTime(2023, 06, 01, 03, 02, 03),
            2000,
            40,
            20,
            0,
            new List<TextTagDto>(),
            TextType.Comics,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Непримечательный, зато просмотров много",
            13000,
            3
        ));

        _texts.Add(new TextInfoDto
        (
            new Guid("4078a860-d800-4187-960e-243e0cf21bd6"),
            "5",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 1",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("4426b840-a8bb-4801-87a1-c09853c7aa23"),
            "6",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 2",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("eb7bbc83-c218-4e82-a78c-efb5f30aba55"),
            "7",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 3",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("67e75ddc-3b9c-47f7-82b7-268444a62b52"),
            "8",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 4",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("b05ee3fe-6bbc-45ee-8cbc-d2a29e39f445"),
            "9",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 5",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("721f1dca-9da2-468b-93e0-caf67abe358f"),
            "10",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 6",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("74c874b1-c2ea-4f28-93d4-8927ccbeaaa4"),
            "11",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 7",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("acd74a98-bd49-4367-8714-9d4be3af63e0"),
            "12",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 8",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("8532f473-d72b-4de2-a41a-06bb1ca896d9"),
            "13",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 9",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
        
        _texts.Add(new TextInfoDto
        (
            new Guid("d681a071-22ba-4f08-834c-83b362403c3a"),
            "14",
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            null,
            new CreatureDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 10",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>(),
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>(),
            "Тестовый рассказ",
            10000,
            3
        ));
    }

    /// <summary>
    /// Get text info by ID
    /// </summary>
    [Route("api/Texts/GetInfo/{id}")]
    [HttpGet]
    public async Task<ActionResult<TextInfoResponse>> GetTextInfoAsync(Guid id)
    {
        return Ok
        (
            new TextInfoResponse
            (
                _texts
                .SingleOrDefault(t => t.Id == id)
            )
        );
    }

    /// <summary>
    /// Get latest texts
    /// </summary>
    [Route("api/Texts/Latest")]
    [HttpGet]
    public async Task<ActionResult<TextsInfosListResponse>> GetLatestTextsAsync(int skip, int take)
    {
        if (skip < 0)
        {
            return BadRequest("Skip must be non-negative.");
        }

        if (take <= 0)
        {
            return BadRequest("Take must be positive.");
        }

        if (skip > _texts.Count())
        {
            return BadRequest("Skip too big.");
        }

        var textInfos = _texts
            .OrderByDescending(t => t.AddTime)
            .Skip(skip)
            .Take(take)
            .ToList();

        var remaining = Math.Max(0, _texts.Count - (skip + take));
        
        return Ok(new TextsInfosListResponse(textInfos, remaining));
    }
    
    /// <summary>
    /// Get most popular texts
    /// </summary>
    [Route("api/Texts/Popular")]
    [HttpGet]
    public async Task<ActionResult<TextsInfosListResponse>> GetPopularTextsAsync(int skip, int take)
    {
        if (skip < 0)
        {
            return BadRequest("Skip must be non-negative.");
        }

        if (take <= 0)
        {
            return BadRequest("Take must be positive.");
        }

        if (skip > _texts.Count())
        {
            return BadRequest("Skip too big.");
        }

        var textInfos = _texts
            .OrderByDescending(t => t.ViewsCount)
            .Skip(skip)
            .Take(take)
            .ToList();

        var remaining = Math.Max(0, _texts.Count - (skip + take));
        
        return Ok(new TextsInfosListResponse(textInfos, remaining));
    }
}