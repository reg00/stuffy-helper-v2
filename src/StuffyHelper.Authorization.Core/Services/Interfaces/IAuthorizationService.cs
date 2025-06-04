using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Core.Services.Interfaces;

/// <summary>
/// Authorization service
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Login into stuffy helper
    /// </summary>
    /// <param name="model">Login nmodel</param>
    /// <param name="httpContext">Http context</param>
    Task<JwtSecurityToken> Login(LoginModel model, HttpContext httpContext);

    /// <summary>
    /// Register user in stuffy helper
    /// </summary>
    /// <param name="model">Register model</param>
    Task<string> Register(RegisterModel model);

    /// <summary>
    /// Return all exists roles
    /// </summary>
    IReadOnlyList<IdentityRole> GetRoles();

    /// <summary>
    /// Identify is user an admin
    /// </summary>
    /// <param name="user">User claims</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<bool> CheckUserIsAdmin(ClaimsPrincipal user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get user by token
    /// </summary>
    /// <param name="user">User claims</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<GetUserEntry> GetUserByToken(ClaimsPrincipal user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Return users by username
    /// </summary>
    /// <param name="userName">Username</param>
    IReadOnlyList<UserShortEntry> GetUserLogins(string? userName = null);

    /// <summary>
    /// Delete user from stuffy helper by username or id
    /// </summary>
    /// <param name="userName">Username</param>
    /// <param name="userId">User id</param>
    Task DeleteUser(string? userName = null, string? userId = null);

    /// <summary>
    /// Update user info
    /// </summary>
    /// <param name="user">User claims</param>
    /// <param name="model">Update model</param>
    Task<GetUserEntry> UpdateUser(ClaimsPrincipal user, UpdateModel model);

    /// <summary>
    /// Update avatar of user
    /// </summary>
    /// <param name="user">User claims</param>
    /// <param name="file">Avatar file</param>
    Task UpdateAvatar(ClaimsPrincipal user, IFormFile file);

    /// <summary>
    /// Remove user's avatar
    /// </summary>
    /// <param name="user">User claims</param>
    Task RemoveAvatar(ClaimsPrincipal user);

    /// <summary>
    /// Return user by username
    /// </summary>
    /// <param name="userName">Username</param>
    Task<GetUserEntry> GetUserByName(string userName);

    /// <summary>
    /// Get user by its id
    /// </summary>
    /// <param name="userId">User id</param>
    Task<GetUserEntry> GetUserById(string userId);

    /// <summary>
    /// Confirm email in stuffy helper
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="code">Confirm code</param>
    Task<GetUserEntry> ConfirmEmail(string login, string code);

    /// <summary>
    /// When user forgot pass, he can send request to reset it
    /// </summary>
    /// <param name="model">Forgot password model</param>
    Task<(string name, string code)> ForgotPasswordAsync(ForgotPasswordModel model);

    /// <summary>
    /// Reset password request
    /// </summary>
    /// <param name="model">Reset password model</param>
    Task ResetPasswordAsync(ResetPasswordModel model);
}