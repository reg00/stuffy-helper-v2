using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for resed password
/// </summary>
public class ResetPasswordModel
{
    /// <summary>
    /// User email
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// New password
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; init; } = string.Empty;

    /// <summary>
    /// Confirm password
    /// </summary>
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// Code
    /// </summary>
    public string Code { get; init; } = string.Empty;
}