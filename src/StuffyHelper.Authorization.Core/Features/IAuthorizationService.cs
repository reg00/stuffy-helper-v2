﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StuffyHelper.Authorization.Core.Features
{
    public interface IAuthorizationService
    {
        Task<JwtSecurityToken> Login(LoginModel model);

        Task Logout(HttpContext httpContext);

        Task<UserEntry> Register(RegisterModel model, ClaimsPrincipal user);

        IEnumerable<IdentityRole> GetRoles();

        bool? CheckUserIsAdmin(ClaimsPrincipal user);

        Task<UserEntry> GetUserByToken(ClaimsPrincipal user, CancellationToken cancellationToken = default);

        IEnumerable<UserEntry> GetUserLogins(string userName = null);

        Task DeleteUser(string userName = null, string userId = null);

        Task<UserEntry> UpdateUser(string userName, UpdateModel model);

        Task<UserEntry> GetUser(string userName = null, string userId = null);
    }
}