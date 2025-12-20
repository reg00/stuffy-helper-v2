using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for register user
/// </summary>
public class RegisterModel
{
    /// <summary>
    /// Username
    /// </summary>
    [Required(ErrorMessage = "Необходимо заполнить поле логина")]
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// User email
    /// </summary>
    [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
    [Required(ErrorMessage = "Необходимо заполнить поле электронной почты")]
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// User password
    /// </summary>
    [Required(ErrorMessage = "Необходимо заполнить поле пароля")]
    public string Password { get; init; } = string.Empty;
}