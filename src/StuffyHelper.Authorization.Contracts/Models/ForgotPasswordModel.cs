using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for forgot password request
/// </summary>
public class ForgotPasswordModel
{
    /// <summary>
    /// Email
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
}