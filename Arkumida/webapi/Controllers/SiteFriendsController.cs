using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.Api.DTOs;
using webapi.Models.Api.Responses;

namespace webapi.Controllers;

/// <summary>
/// Controller, returning information about site friends
/// </summary>
[Authorize]
[ApiController]
public class SiteFriendsController : ControllerBase
{
    /// <summary>
    /// Get site friends
    /// </summary>
    [AllowAnonymous]
    [Route("api/SiteFriends/Get")]
    [HttpGet]
    public async Task<ActionResult<SiteFriendsResponse>> GetSiteFriendsAsync()
    {
        var friends = new List<SiteFriendDto>()
        {
            new SiteFriendDto(new Guid("3d5a74a2-3029-457b-b457-454f4193baf8"), "Рыжий лис", "mailto:red-fox0@mail.ru?subject=furtails.pw", "red-fox0@mail.ru"),
            new SiteFriendDto(new Guid("ce0247d6-3b9c-42c5-aa14-5dc2133a190d"), "MoDErahN", "mailto:MoDErahN@yahoo.com", "MoDErahN@yahoo.com"),
            new SiteFriendDto(new Guid("b2c01536-b1ed-45dd-96d5-3478b41ad1c8"), "Дремлющий", "mailto:nsf0693@rambler.ru", "nsf0693@rambler.ru"),
            new SiteFriendDto(new Guid("87bde91a-b43a-4b38-9742-73382fae92af"), "Первозвери", "https://fossa.life", "Блог Первозверей"),
            new SiteFriendDto(new Guid("7c749cc8-2275-41b6-bb84-aae8dc6b228a"), "Aaz", "mailto:orkas@mail.ru", "orkas@mail.ru"),
            new SiteFriendDto(new Guid("85d4b7bb-55ba-4fe8-acc7-560409bd4314"), "Alien Xenomorph", "mailto:xenomorph@mail.ru", "xenomorph@mail.ru"),
            new SiteFriendDto(new Guid("f799d3b6-2525-4bb7-abc1-4798ea360aee"), "vervolk Davide Norton", "mailto:Davidenorton@googlemail.com", "Davidenorton@googlemail.com")
        };
        
        return Ok(new SiteFriendsResponse(friends));
    }
}