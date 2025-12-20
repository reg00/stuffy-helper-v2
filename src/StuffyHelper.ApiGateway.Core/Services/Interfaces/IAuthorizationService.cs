using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces;

/// <summary>
/// Authorization service
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Логин
    /// </summary>
    /// <param name="model">Login model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<string> Login(LoginModel model, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="model">Register model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<string> Register(RegisterModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение ролей
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IReadOnlyList<IdentityRole>> GetRoles(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверка является ли пользователь администратором
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<bool> CheckUserIsAdmin(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение списка логинов
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="userName">User name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IReadOnlyList<UserShortEntry>> GetUserLogins(string token, string? userName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление автарки пользователя
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="file">Avatar file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAvatar(string token, IFormFile file, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Удаление аватара пользователя
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task RemoveAvatar(string token, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение информации о пользователе по Id
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<GetUserEntry> GetUserById(string userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Подтверждение почты
    /// </summary>
    /// <param name="login">Login</param>
    /// <param name="code">Code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task ConfirmEmail(string login, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// "Забыл пароль"
    /// </summary>
    /// <param name="model">Model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<string> ForgotPasswordAsync(ForgotPasswordModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сброс пароля
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="code">Code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<ResetPasswordResult> ResetPasswordAsync(string email, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Подтверждение сброса пароля
    /// </summary>
    /// <param name="model">Model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<string> ConfirmResetPasswordAsync(ResetPasswordModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение информации он аккаунте
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<GetUserEntry> GetAccountInfoAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление информации о пользователе
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="model">Model</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<GetUserEntry> UpdateUserAsync(string token, UpdateModel model, CancellationToken cancellationToken = default);
}