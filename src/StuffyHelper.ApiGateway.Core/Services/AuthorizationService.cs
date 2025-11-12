using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services;

/// <inheritdoc />
public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationClient _authorizationClient;

    /// <summary>
    /// Ctor.
    /// </summary>
    public AuthorizationService(IAuthorizationClient authorizationClient)
    {
        _authorizationClient = authorizationClient;
    }

    /// <inheritdoc />
    public async Task<LoginResponse> Login(LoginModel model, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.Login(model, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task Logout(string token, CancellationToken cancellationToken = default)
    {
        await _authorizationClient.Logout(token, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<LoginResponse> Refresh(CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.Refresh(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> Register(RegisterModel model, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.Register(model, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<IdentityRole>> GetRoles(string token, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.GetRoles(token, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> CheckUserIsAdmin(string token, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.CheckUserIsAdmin(token, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserShortEntry>> GetUserLogins(string token, string? userName = null,
        CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.GetUserLogins(token, userName, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAvatar(string token, IFormFile file, CancellationToken cancellationToken = default)
    {
        await _authorizationClient.EditAvatarAsync(token, file, cancellationToken);
    }

    /// <inheritdoc />
    public async Task RemoveAvatar(string token, CancellationToken cancellationToken = default)
    {
        await _authorizationClient.RemoveAvatarAsync(token, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<GetUserEntry> GetUserById(string userId, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.GetUserById(userId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task ConfirmEmail(string login, string code, CancellationToken cancellationToken = default)
    {
        await _authorizationClient.ConfirmEmail(login, code, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> ForgotPasswordAsync(ForgotPasswordModel model, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.ForgotPassword(model, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ResetPasswordResult> ResetPasswordAsync(string email, string code, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.ResetPassword(email, code, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> ConfirmResetPasswordAsync(ResetPasswordModel model, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.ConfirmResetPassword(model, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<GetUserEntry> GetAccountInfoAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.GetAccountInfoAsync(token, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<GetUserEntry> UpdateUserAsync(string token, UpdateModel model, CancellationToken cancellationToken = default)
    {
        return await _authorizationClient.EditUserAsync(token, model, cancellationToken);
    }
}