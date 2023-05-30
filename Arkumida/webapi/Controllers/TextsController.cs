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
                        SpecialTextType.Normal
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
                        SpecialTextType.Normal
                    ));
            
            default:
                return BadRequest();
        }
        
        //return Ok(new VersionInfoResponse("Аркумида-А мод. 3", "https://github.com/WhiteFossa/Arkumida"));
    }
}