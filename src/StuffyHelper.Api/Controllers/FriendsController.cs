using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Authorization.Core1.Features.Friends;
using StuffyHelper.Authorization.Core1.Models;
using StuffyHelper.Authorization.Core1.Models.User;
using StuffyHelper.Core.Features.Common;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly IFriendService _friendshipService;

        public FriendsController(IFriendService friendshipService)
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
}
