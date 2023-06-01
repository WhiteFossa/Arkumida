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
    /// <summary>
    /// Get text info by ID
    /// </summary>
    [Route("api/Texts/GetInfo/{id}")]
    [HttpGet]
    public async Task<ActionResult<TextInfoResponse>> GetTextInfoAsync(Guid id)
    {
        switch (id.ToString().ToLower())
        {
            case "0e9fc697-31d6-4776-bf7e-05950936b4de":
                return new TextInfoResponse(
                    new TextInfoDto
                    (
                        id,
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
                    ));
            
            case "185d7cc9-83e7-4de3-b1d8-5f8d50af6a88":
                return new TextInfoResponse(
                    new TextInfoDto
                    (
                        id,
                        "2",
                        new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
                        "Унылый текст, который никто не читает",
                        DateTime.UtcNow,
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
                    ));
            
            case "e77c2183-2c49-4e56-a7a5-576f674f1403":
                return new TextInfoResponse(
                    new TextInfoDto
                    (
                        id,
                        "3",
                        new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
                        "Обычный поэтический гетеройифф с конкурса",
                        DateTime.UtcNow,
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
                    ));
            
            case "a4ade7ba-7c67-4ba7-abb9-06d11b98d2b6":
                return new TextInfoResponse(
                    new TextInfoDto
                    (
                        id,
                        "4",
                        new AuthorDto(new Guid("6ba6318a-d884-45ca-b50e-0fe8ecff4300"), "1", "Фосса"),
                        "Ничем непримечательный гомойифф",
                        DateTime.UtcNow,
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
                    ));
            
            default:
                return BadRequest();
        }
    }
}