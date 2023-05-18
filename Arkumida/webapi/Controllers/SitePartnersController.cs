using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Controller to work with site partners
/// </summary>
[ApiController]
public class SitePartnersController : ControllerBase
{
    /// <summary>
    /// Get site partners
    /// </summary>
    [Route("api/SitePartners/Get")]
    [HttpGet]
    public async Task<ActionResult<SitePartnersResponse>> GetSitePartnersAsync()
    {
        var partners = new List<SitePartnerDto>()
        {
            new SitePartnerDto(new Guid("04e054da-03d3-4114-991f-7e2bb0f483eb"), "http://furry-world.ru/", "Furry World", "/images/banners/furry-world.ru.gif", "Furry World"),
            new SitePartnerDto(new Guid("89c6962b-c83b-4374-ae12-c21ca4e42e8f"), "http://foxcomix.ru/", "Новые стрипы каждый понедельник", "/images/banners/foxcomix.gif", "Лисий комикс"),
            new SitePartnerDto(new Guid("28d58366-788e-495b-96f3-dd537ff83025"), "http://sssradio.ru/", "Субкультурное радио", "/images/banners/sssradio.ru.jpg", "Субкультурное радио"),
            new SitePartnerDto(new Guid("4813e160-784e-42cd-a532-517e115f9736"), "http://www.onegai.in/", "Фурри Йифф Хентай Юри Яой - Онегай!", "/images/banners/onegai.gif", "Фурри Йифф Хентай Юри Яой - Онегай!")
        };

        return Ok(new SitePartnersResponse(partners));
    }
}