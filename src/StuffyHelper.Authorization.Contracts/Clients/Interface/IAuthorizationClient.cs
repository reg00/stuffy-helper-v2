using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Contracts.Clients.Interface;

public interface IAuthorizationClient
{
    public Task<string> Register(
        RegisterModel body,
        CancellationToken cancellationToken = default);

    public Task ConfirmEmail(
        string login,
        string code,
        CancellationToken cancellationToken = default);

    public Task<GetUserEntry> Login(
        LoginModel body,
        CancellationToken cancellationToken = default);

    public Task Logout(
        string token,
        CancellationToken cancellationToken = default);

    public Task<string> ForgotPassword(
        ForgotPasswordModel body,
        CancellationToken cancellationToken = default);

    public Task<ResetPasswordResult> ResetPassword(
        string email,
        string code,
        CancellationToken cancellationToken = default);

    public Task<string> ConfirmResetPassword(
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