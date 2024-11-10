using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;

namespace StuffyHelper.Authorization.Api.Controllers;

[Authorize]
public class FriendController : Controller
{
    private readonly IFriendService _friendshipService;

    public FriendController(IFriendService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    /// <summary>
    /// Получение списка друзей
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(AuthResponse<UserShortEntry>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Route(KnownRoutes.GetFriendsRoute)]
    public async Task<IActionResult> GetAsync(int limit = 20, int offset = 0)
    {
        var friends = await _friendshipService.GetFriends(User, limit, offset, HttpContext.RequestAborted);

        return StatusCode((int)HttpStatusCode.OK, friends);
    }
}