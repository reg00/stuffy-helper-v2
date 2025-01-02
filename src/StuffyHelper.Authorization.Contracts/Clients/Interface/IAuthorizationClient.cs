using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Contracts.Clients.Interface;

public interface IAuthorizationClient
{
    public Task<string> Register(
        string token,
        RegisterModel body,
        CancellationToken cancellationToken = default);

    public Task ConfirmEmail(
        string token,
        string login,
        string code,
        CancellationToken cancellationToken = default);

    public Task<GetUserEntry> Login(
        string token,
        LoginModel body,
        CancellationToken cancellationToken = default);

    public Task Logout(
        string token,
        CancellationToken cancellationToken = default);

    public Task<string> ForgotPassword(
        string token,
        ForgotPasswordModel body,
        CancellationToken cancellationToken = default);

    public Task<ResetPasswordResult> ResetPassword(
        string token,
        string email,
        string code,
        CancellationToken cancellationToken = default);

    public Task<string> ConfirmResetPassword(
        string token,
        ResetPasswordModel body,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<IdentityRole>> GetRoles(
        string token,
        CancellationToken cancellationToken = default);

    public Task<bool> CheckUserIsAdmin(
        string token,
        CancellationToken cancellationToken = default);

    public Task<GetUserEntry> GetAccountInfoAsync(
        string token,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<UserShortEntry>> GetUserLogins(
        string token,
        string? userName = null,
        CancellationToken cancellationToken = default);

    public Task<GetUserEntry> GetUserById(
        string token,
        string userId,
        CancellationToken cancellationToken = default);
    
    public Task<GetUserEntry> EditUserAsync(
        string token,
        UpdateModel body,
        CancellationToken cancellationToken = default);

    public Task EditAvatarAsync(
        string token,
        IFormFile file,
        CancellationToken cancellationToken = default);

    public Task RemoveAvatarAsync(
        string token,
        CancellationToken cancellationToken = default);
}