﻿using System.Net;
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
    [ProducesResponseType(typeof(Response<UserShortEntry>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Route(KnownRoutes.GetFriendsRoute)]
    public async Task<Response<UserShortEntry>> GetAsync(int limit = 20, int offset = 0)
    {
        return await _friendshipService.GetFriends(User, limit, offset, HttpContext.RequestAborted);
    }
}