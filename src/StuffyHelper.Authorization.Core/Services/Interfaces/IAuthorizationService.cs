using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Core.Services.Interfaces;

public interface IAuthorizationService
{
    Task<JwtSecurityToken> Login(LoginModel model, HttpContext httpContext);

    Task Logout(HttpContext httpContext);

    Task<string> Register(RegisterModel model);

    IEnumerable<IdentityRole> GetRoles();

    Task<bool> CheckUserIsAdmin(ClaimsPrincipal user, CancellationToken cancellationToken = default);

    Task<UserEntry> GetUserByToken(ClaimsPrincipal user, CancellationToken cancellationToken = default);

    IEnumerable<UserShortEntry> GetUserLogins(string? userName = null);

    Task DeleteUser(string? userName = null, string? userId = null);

    Task<UserEntry> UpdateUser(ClaimsPrincipal user, UpdateModel model);

    Task UpdateAvatar(ClaimsPrincipal user, IFormFile file);

    Task RemoveAvatar(ClaimsPrincipal user);

    Task<UserEntry> GetUserByName(string userName);

    Task<UserEntry> GetUserById(string userId);

    Task<UserEntry> ConfirmEmail(string login, string code);

    Task<(string name, string code)> ForgotPasswordAsync(ForgotPasswordModel model);

    Task ResetPasswordAsync(ResetPasswordModel model);
}