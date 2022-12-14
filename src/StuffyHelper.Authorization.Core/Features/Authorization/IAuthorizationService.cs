using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StuffyHelper.Authorization.Core.Features
{
    public interface IAuthorizationService
    {
        Task<JwtSecurityToken> Login(LoginModel model, HttpContext httpContext);

        Task Logout(HttpContext httpContext);

        Task<string> Register(RegisterModel model);

        IEnumerable<IdentityRole> GetRoles();

        Task<bool> CheckUserIsAdmin(ClaimsPrincipal user, CancellationToken cancellationToken = default);

        Task<bool> CheckUserIsAdmin(string userName, CancellationToken cancellationToken = default);

        Task<UserEntry> GetUserByToken(ClaimsPrincipal user, CancellationToken cancellationToken = default);

        IEnumerable<UserEntry> GetUserLogins(string userName = null);

        Task DeleteUser(string userName = null, string userId = null);

        Task<UserEntry> UpdateUser(ClaimsPrincipal user, UpdateModel model);

        Task<UserEntry> GetUser(string userName = null, string userId = null);

        Task<UserEntry> ConfirmEmail(string login, string code);

        Task<(string name, string code)> ForgotPasswordAsync(ForgotPasswordModel model);

        Task ResetPasswordAsync(ResetPasswordModel model);
    }
}
