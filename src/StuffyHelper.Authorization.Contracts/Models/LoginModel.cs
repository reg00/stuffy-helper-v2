using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for login
/// </summary>
public class LoginModel
{
    /// <summary>
    /// Username
    /// </summary>
    [Required(ErrorMessage = "Необходимо заполнить логин")]
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Необходимо заполнить пароль")]
    public string Password { get; init; } = string.Empty;
}