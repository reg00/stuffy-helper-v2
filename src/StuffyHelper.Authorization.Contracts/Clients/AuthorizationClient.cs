using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RestSharp;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Client.Helpers;
using StuffyHelper.Common.Web;

namespace StuffyHelper.Authorization.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Authorization.Contracts.Clients.Interface.IAuthorizationClient" />
public class AuthorizationClient : ApiClientBase, IAuthorizationClient
{
    /// <inheritdoc />
    public AuthorizationClient(string baseUrl) : base(baseUrl)
    {
        
    }

    /// <inheritdoc />
    public Task<string> Register(
        RegisterModel body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.RegisterRoute)
            .AddJsonBody(body);

        return Post<string>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task ConfirmEmail(
        string login,
        string code,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.EmailConfirmRoute)
            .AddQueryParameter(nameof(login), login)
            .AddQueryParameter(nameof(code), code);

        return Get(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<string> Login(
        LoginModel body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.LoginRoute)
            .AddJsonBody(body);

        return Post<string>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<string> ForgotPassword(
        ForgotPasswordModel body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.ResetPasswordRoute)
            .AddJsonBody(body);

        return Post<string>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<ResetPasswordResult> ResetPassword(
        string email,
        string code,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.ResetPasswordConfirmRoute)
            .AddQueryParameter(nameof(email), email)
            .AddQueryParameter(nameof(code), code);

        return Get<ResetPasswordResult>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<string> ConfirmResetPassword(
        ResetPasswordModel body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.ResetPasswordConfirmRoute)
            .AddJsonBody(body);

        return Post<string>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<IReadOnlyList<IdentityRole>> GetRoles(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.RolesRoute)
            .AddBearerToken(token);

        return Get<IReadOnlyList<IdentityRole>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<bool> CheckUserIsAdmin(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.IsAdminRoute)
            .AddBearerToken(token);

        return Get<bool>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<GetUserEntry> GetAccountInfoAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.AccountRoute)
            .AddBearerToken(token);

        return Get<GetUserEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<GetUserEntry> GetUserById(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{KnownRoutes.UserLoginsRoute}/{userId}");

        return await Get<GetUserEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<IReadOnlyList<UserShortEntry>> GetUserLogins(
        string token,
        string? userName = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.UserLoginsRoute)
            .AddQueryParameter(nameof(userName), userName)
            .AddBearerToken(token);

        return Get<IReadOnlyList<UserShortEntry>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<GetUserEntry> EditUserAsync(
        string token,
        UpdateModel body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.EditUserRoute)
            .AddJsonBody(body)
            .AddBearerToken(token);

        return Patch<GetUserEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task EditAvatarAsync(
        string token,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.AvatarRoute)
            .AddFile(nameof(file), file.ToFileParam())
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task RemoveAvatarAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.AvatarRoute)
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
}