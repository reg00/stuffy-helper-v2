using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Contracts.Clients.Interface;

/// <summary>
/// Interface for work with auth
/// </summary>
public interface IAuthorizationClient
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="body">Register model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<string> Register(
        RegisterModel body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Подтверждение почты
    /// </summary>
    /// <param name="login">Login</param>
    /// <param name="code">Code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task ConfirmEmail(
        string login,
        string code,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Логин
    /// </summary>
    /// <param name="body">Login model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<LoginResponse> Login(
        LoginModel body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Logout from system
    /// </summary>
    public Task Logout(
        string token,
        CancellationToken cancellationToken = default);

    
    /// <summary>
    /// Gets a refresh token
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<LoginResponse> Refresh(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// "Забыл пароль"
    /// </summary>
    /// <param name="body">Model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<string> ForgotPassword(
        ForgotPasswordModel body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Сброс пароля
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="code">Code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<ResetPasswordResult> ResetPassword(
        string email,
        string code,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Подтверждение сброса пароля
    /// </summary>
    /// <param name="body">Model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<string> ConfirmResetPassword(
        ResetPasswordModel body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение ролей
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<IReadOnlyList<IdentityRole>> GetRoles(
        string token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверка является ли пользователь администратором
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<bool> CheckUserIsAdmin(
        string token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение информации он аккаунте
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<GetUserEntry> GetAccountInfoAsync(
        string token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение списка логинов
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="userName">User name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<IReadOnlyList<UserShortEntry>> GetUserLogins(
        string token,
        string? userName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение информации о пользователе по Id
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<GetUserEntry> GetUserById(
        string userId,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Обновление информации о пользователе
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="body">Model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<GetUserEntry> EditUserAsync(
        string token,
        UpdateModel body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление автарки пользователя
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="file">Avatar file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task EditAvatarAsync(
        string token,
        IFormFile file,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление аватара пользователя
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task RemoveAvatarAsync(
        string token,
        CancellationToken cancellationToken = default);
}