using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Enums;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with texts
/// </summary>
[ApiController]
public class TextsController : ControllerBase
{
    private readonly IReadOnlyCollection<TextInfoDto> _texts = new List<TextInfoDto>()
    {
        new TextInfoDto
        (
            new Guid("0e9fc697-31d6-4776-bf7e-05950936b4de"),
            "1",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Сказка о том, как Фосса себя за хвост укусили",
            DateTime.UtcNow,
            1337,
            27,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("fa6ef372-6182-425f-94e0-1148ddfd07de"), "1", "F/F"),
                new TextTagDto(new Guid("14607fef-7415-4eb6-ba92-eaa5b66f3592"), "2", "Снафф")
            },
            TextType.Story,
            SpecialTextType.Snuff,
            new List<TextIconDto>(),
            new List<TextIconDto>() { new TextIconDto(TextIconType.Illustrations) }
        ),
        
        new TextInfoDto
        (
            new Guid("185d7cc9-83e7-4de3-b1d8-5f8d50af6a88"),
            "2",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Унылый текст, который никто не читает",
            new DateTime(2023, 06, 01, 01, 02, 03),
            3,
            0,
            0,
            10,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Novel,
            SpecialTextType.Sandbox,
            new List<TextIconDto>(),
            new List<TextIconDto>() { new TextIconDto(TextIconType.Incomplete) }
        ),
        
        new TextInfoDto
        (
            new Guid("e77c2183-2c49-4e56-a7a5-576f674f1403"),
            "3",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Обычный поэтический гетеройифф с конкурса",
            new DateTime(2023, 06, 01, 02, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Poetry,
            SpecialTextType.Contest,
            new List<TextIconDto>() { new TextIconDto(TextIconType.MLP) },
            new List<TextIconDto>() { new TextIconDto(TextIconType.Series, "/texts/bySeries/2b1d2462-084a-4566-b9e1-c68411b5e7e0") }
        ),
        
        new TextInfoDto
        (
            new Guid("a4ade7ba-7c67-4ba7-abb9-06d11b98d2b6"),
            "4",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Ничем непримечательный гомойифф",
            new DateTime(2023, 06, 01, 03, 02, 03),
            2000,
            40,
            20,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("00bb5088-27c6-4e8d-979c-da74df541b28"), "4", "M/M")
            },
            TextType.Comics,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("4078a860-d800-4187-960e-243e0cf21bd6"),
            "5",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 1",
            new DateTime(2023, 06, 01, 04, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("d08227a6-3a2a-4d66-9c5a-3c6d748a7599"),
            "6",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 2",
            new DateTime(2023, 06, 01, 05, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("f23c98a2-c275-4172-989c-b145a3db9ff8"),
            "7",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 3",
            new DateTime(2023, 06, 01, 06, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("0b782220-99bc-471a-869e-55624ef57b91"),
            "8",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 4",
            new DateTime(2023, 06, 01, 07, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("9c2630ce-4d1b-4c71-8b3f-9f89f048f62f"),
            "9",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 5",
            new DateTime(2023, 06, 01, 08, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("29252200-989c-4e4e-919a-3bce60100315"),
            "10",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 6",
            new DateTime(2023, 06, 01, 09, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("a604ae62-2240-42b4-9341-c0e0f6bcf30d"),
            "11",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 7",
            new DateTime(2023, 06, 01, 10, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("ce63bc6f-378a-4139-95f7-40cb3757fea0"),
            "12",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 8",
            new DateTime(2023, 06, 01, 11, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("8eed77f0-969a-4493-93eb-84f04228175b"),
            "13",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 9",
            new DateTime(2023, 06, 01, 12, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("aba15929-7a59-4a32-994d-ce8ea1c8ec0a"),
            "14",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 10",
            new DateTime(2023, 06, 01, 13, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("7395ea64-5684-42ab-bc02-392251883ef0"),
            "15",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 11",
            new DateTime(2023, 06, 01, 14, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("ae1bad44-fad6-44b6-bdea-a7777d8de451"),
            "16",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 12",
            new DateTime(2023, 06, 01, 15, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("369283b7-0f00-4e2c-98b8-6dc647f7da3e"),
            "17",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 13",
            new DateTime(2023, 06, 01, 16, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("979b374a-bcc4-4b88-8c15-8cd0756f0e8e"),
            "18",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 14",
            new DateTime(2023, 06, 01, 17, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("d0c4e825-2b7c-4168-88c5-c9853fdff8d8"),
            "19",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 15",
            new DateTime(2023, 06, 01, 18, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("9dfc470a-72fa-43b3-a0e3-2c75ab74b31c"),
            "20",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 16",
            new DateTime(2023, 06, 01, 19, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("ad9ff7c8-bd38-464c-8544-148376c6a172"),
            "21",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 17",
            new DateTime(2023, 06, 01, 20, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        ),
        
        new TextInfoDto
        (
            new Guid("ca5999e8-92e0-4f04-b41a-090ae759c384"),
            "22",
            new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
            "Тестовый рассказ 18",
            new DateTime(2023, 06, 01, 21, 02, 03),
            1000,
            20,
            10,
            0,
            new List<TextTagDto>()
            {
                new TextTagDto(new Guid("5a8c370d-aed3-41e1-adf6-175a56388e7f"), "3", "M/F")
            },
            TextType.Story,
            SpecialTextType.Normal,
            new List<TextIconDto>(),
            new List<TextIconDto>()
        )
    };

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
    public async Task<ActionResult<TextsInfosListResponse>> GetLatestTexts(int skip, int take)
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
}